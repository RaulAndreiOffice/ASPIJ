from fastapi import FastAPI
from pydantic import BaseModel
import pandas as pd
import numpy as np
import joblib
import pickle
from tensorflow.keras.models import load_model

app = FastAPI()

model = load_model("whoop_avg_hr_model.h5")
scaler = joblib.load("whoop_scaler.pkl")

with open("whoop_columns.pkl", "rb") as f:
    trained_columns = pickle.load(f)


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


@app.post("/predict")
def predict(req: PredictRequest):
    resting_hr = req.resting_heart_rate if req.resting_heart_rate is not None else 60.0
    recovery = req.recovery_score if req.recovery_score is not None else 50.0

    raw_input = pd.DataFrame([{
        "age": req.age,
        "gender": req.gender,
        "weight_kg": req.weight_kg,
        "height_cm": req.height_cm,
        "resting_heart_rate": resting_hr,
        "recovery_score": recovery,
        "activity_type": req.activity_type,
        "activity_duration_min": req.activity_duration_min
    }])

    categorical_cols = ["gender", "activity_type"]
    raw_encoded = pd.get_dummies(raw_input, columns=categorical_cols)

    for col in trained_columns:
        if col not in raw_encoded.columns:
            raw_encoded[col] = 0

    raw_encoded = raw_encoded[trained_columns]

    X_scaled = scaler.transform(raw_encoded)
    bpm_pred = float(model.predict(X_scaled, verbose=0)[0][0])

    response = {
        "predicted_avg_heart_rate": round(bpm_pred, 2)
    }

    if req.heart_rate_measured is not None:
        delta = abs(bpm_pred - req.heart_rate_measured)
        response["heart_rate_measured"] = req.heart_rate_measured
        response["delta_bpm"] = round(delta, 2)

    return response