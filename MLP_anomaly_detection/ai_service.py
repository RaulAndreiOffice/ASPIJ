from fastapi import FastAPI
from pydantic import BaseModel
import pandas as pd
import numpy as np
import joblib
import pickle
from tensorflow.keras.models import load_model

app = FastAPI()

model = load_model("whoop_avg_hr_model.h5", compile=False)
scaler = joblib.load("whoop_scaler.pkl")

with open("whoop_columns.pkl", "rb") as f:
    trained_columns = pickle.load(f)



# FUZZY LOGIC HELPERS
# Triangular membership function: returns degree [0.0 – 1.0]


def trimf(x: float, a: float, b: float, c: float) -> float:
    """Triangular membership: 0 at a, peak 1 at b, 0 at c."""
    if x <= a or x >= c:
        return 0.0
    if x <= b:
        return (x - a) / (b - a)
    return (c - x) / (c - b)


def trapmf(x: float, a: float, b: float, c: float, d: float) -> float:
    """Trapezoidal membership: rises a→b, flat b→c, falls c→d."""
    if x <= a or x >= d:
        return 0.0
    if x <= b:
        return (x - a) / (b - a)
    if x <= c:
        return 1.0
    return (d - x) / (d - c)



# FUZZY RULE 1 – INJURY RISK
# Input:  delta_bpm  (|predicted − measured|)
# Output: injury_risk_score  0–100  →  label + advice
#
# Membership sets for delta_bpm:
#   low    [0, 0, 5, 10]   – normal variation, no concern
#   medium [5, 15, 25]     – elevated, monitor closely
#   high   [20, 30, 999]   – significant overload / injury risk


def fuzzy_injury_risk(delta_bpm: float) -> dict:
    mu_low    = trapmf(delta_bpm, 0,  0,  5,  10)
    mu_medium = trimf (delta_bpm, 5,  15, 25)
    mu_high   = trapmf(delta_bpm, 20, 30, 999, 1000)

    # Defuzzification – weighted average of crisp output centroids
    # low→10,  medium→50,  high→90
    num = mu_low * 10 + mu_medium * 50 + mu_high * 90
    den = mu_low + mu_medium + mu_high

    score = round(num / den, 1) if den > 0 else 0.0

    # Label
    if mu_high >= mu_medium and mu_high >= mu_low:
        label  = "HIGH injury risk"
        advice = (
            "Your measured HR is significantly above the predicted safe range. "
            "Stop or greatly reduce intensity. Rest and monitor recovery."
        )
    elif mu_medium >= mu_low:
        label  = "MODERATE injury risk"
        advice = (
            "HR deviation is noticeable. Reduce pace, stay hydrated, "
            "and check how you feel before continuing."
        )
    else:
        label  = "LOW injury risk"
        advice = "HR deviation is within normal range. Keep going safely."

    return {
        "injury_risk_score": score,         # 0–100
        "injury_risk_label": label,
        "injury_risk_advice": advice,
        "fuzzy_memberships_delta": {
            "low":    round(mu_low,    3),
            "medium": round(mu_medium, 3),
            "high":   round(mu_high,   3),
        },
    }



# FUZZY RULE 2 – UNDER-TRAINING
# Input:  measured_bpm  (absolute heart rate during activity)
# Output: under_training_score  0–100  →  label + advice
#
# The idea: if your HR stays very low relative to what a workout should
# produce, you are under-training (not pushing hard enough).
#
# Membership sets for measured_bpm:
#   very_low   [0,  0,  90, 100]   – likely under-training
#   low        [90, 100, 115]      – light effort
#   moderate   [110, 130, 150]     – aerobic zone, healthy
#   high       [145, 165, 999]     – intense, possible overload


def fuzzy_under_training(measured_bpm: float) -> dict:
    mu_very_low = trapmf(measured_bpm, 0,   0,   90,  100)
    mu_low      = trimf (measured_bpm, 90,  100, 115)
    mu_moderate = trimf (measured_bpm, 110, 130, 150)
    mu_high     = trapmf(measured_bpm, 145, 165, 999, 1000)

    # Under-training score: high when bpm is very_low or low
    # Centroids: very_low→90, low→60, moderate→20, high→5
    num = mu_very_low * 90 + mu_low * 60 + mu_moderate * 20 + mu_high * 5
    den = mu_very_low + mu_low + mu_moderate + mu_high

    score = round(num / den, 1) if den > 0 else 0.0

    # Label – dominant membership
    memberships = {
        "very_low": mu_very_low,
        "low":      mu_low,
        "moderate": mu_moderate,
        "high":     mu_high,
    }
    dominant = max(memberships, key=memberships.get)

    if dominant == "very_low":
        label  = "UNDER-TRAINING detected"
        advice = (
            "Heart rate is very low for an active workout. "
            "Increase intensity to reach your aerobic training zone."
        )
    elif dominant == "low":
        label  = "LIGHT effort – possible under-training"
        advice = (
            "You are below the aerobic zone. Consider pushing harder "
            "unless this is an intentional recovery session."
        )
    elif dominant == "moderate":
        label  = "OPTIMAL training zone"
        advice = "Heart rate is in a healthy aerobic range. Great work!"
    else:
        label  = "HIGH intensity – monitor for overload"
        advice = (
            "You are training at high intensity. Make sure recovery "
            "is adequate and watch for fatigue or injury signs."
        )

    return {
        "under_training_score": score,          # 0–100 (higher = more under-trained)
        "under_training_label": label,
        "under_training_advice": advice,
        "fuzzy_memberships_bpm": {
            "very_low": round(mu_very_low, 3),
            "low":      round(mu_low,      3),
            "moderate": round(mu_moderate, 3),
            "high":     round(mu_high,     3),
        },
    }



# REQUEST / RESPONSE MODEL


class PredictRequest(BaseModel):
    age: int
    gender: str
    weight_kg: float
    height_cm: float
    resting_heart_rate: float | None = None
    recovery_score: float | None = None
    activity_type: str
    activity_duration_min: float
    heart_rate_measured: float | None = None



# ENDPOINT


@app.post("/predict")
def predict(req: PredictRequest):
    resting_hr = req.resting_heart_rate if req.resting_heart_rate is not None else 60.0
    recovery   = req.recovery_score     if req.recovery_score     is not None else 50.0

    raw_input = pd.DataFrame([{
        "age":                  req.age,
        "gender":               req.gender,
        "weight_kg":            req.weight_kg,
        "height_cm":            req.height_cm,
        "resting_heart_rate":   resting_hr,
        "recovery_score":       recovery,
        "activity_type":        req.activity_type,
        "activity_duration_min": req.activity_duration_min,
    }])

    categorical_cols = ["gender", "activity_type"]
    raw_encoded = pd.get_dummies(raw_input, columns=categorical_cols)

    for col in trained_columns:
        if col not in raw_encoded.columns:
            raw_encoded[col] = 0

    raw_encoded = raw_encoded[trained_columns]

    X_scaled  = scaler.transform(raw_encoded)
    bpm_pred  = float(model.predict(X_scaled, verbose=0)[0][0])

    response = {
        "predicted_avg_heart_rate": round(bpm_pred, 2),
    }

    #  Fuzzy logic 
    if req.heart_rate_measured is not None:
        delta = abs(bpm_pred - req.heart_rate_measured)

        response["heart_rate_measured"] = req.heart_rate_measured
        response["delta_bpm"]           = round(delta, 2)

        # Rule 1: injury risk based on delta
        response["fuzzy_injury_risk"] = fuzzy_injury_risk(delta)

        # Rule 2: under-training based on absolute measured BPM
        response["fuzzy_under_training"] = fuzzy_under_training(req.heart_rate_measured)

    return response
