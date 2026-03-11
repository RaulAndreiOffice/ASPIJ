import fastapi
from fastapi import FastAPI
from pydantic import BaseModel
import pandas as pd
import numpy as np
import joblib
import pickle
from tensorflow import keras

from FuzzyLogicDetection import fuzzy_injury_risk

app = FastAPI()

# 1. Încărcăm modelul și scaler-ul
model = keras.models.load_model("whoop_avg_hr_model.h5", compile=False)
scaler = joblib.load("whoop_scaler.pkl")

with open("whoop_columns.pkl", "rb") as f:
    input_columns = pickle.load(f)


# 2. Schema datelor primite de la backend
from pydantic import BaseModel

class PredictRequest(BaseModel):
    age: int
    gender: str
    weight_kg: float
    height_cm: float
    resting_heart_rate: float
    recovery_score: float
    activity_type: str
    activity_duration_min: float

 
@app.post("/predict")
def predict(req: PredictRequest):
    # 3. Construim DataFrame EXACT ca la antrenare
    data = {
        "Actual Weight": req.Actual_Weight,
        "Age": req.Age,
        "Duration": req.Duration,
        "BMI": req.BMI,
        "Exercise Intensity": req.Exercise_Intensity,
        "Gender": req.Gender,
        "Exercise": req.Exercise,
        "Weather Conditions": req.Weather_Conditions,
    }
    df = pd.DataFrame([data])

    # 4. Categorice + numerice (la fel ca în scriptul de training)
    categorical_cols = ["Gender", "Exercise", "Weather Conditions"]
    numeric_cols = ["Actual Weight", "Age", "Duration", "BMI", "Exercise Intensity"]

    x_categ = pd.get_dummies(df[categorical_cols], drop_first=False)
    x_num = df[numeric_cols]
    x_all = pd.concat([x_num, x_categ], axis=1)

    # 5. Aliniem cu coloanele de la antrenare
    x_all = x_all.reindex(columns=input_columns, fill_value=0)

    # 6. Scalare
    x_scaled = scaler.transform(x_all)

    # 7. Predicție
    hr_pred = model.predict(x_scaled).flatten()[0]

    # 8. Anomalie (dacă avem puls măsurat)
    anomaly = None
    diff = None
    anomaly_threshold = 20

    fuzzy_score = None

    if req.HeartRate_Measured is not None:
        diff = abs(req.HeartRate_Measured - hr_pred)

        fuzzy_result = fuzzy_injury_risk(hr_pred, req.HeartRate_Measured)
        fuzzy_score = fuzzy_result["risk_score"]
        anomaly = fuzzy_result["decision"] == "High Risk for Injury"


    return {
    "heart_rate_predicted": float(hr_pred),
    "difference": float(diff) if diff is not None else None,
    "fuzzy_score": float(fuzzy_score) if fuzzy_score is not None else None,
    "injury_risk": anomaly
  }


if __name__ == "__main__":
    import uvicorn
    uvicorn.run("ai_service:app", host="127.0.0.1", port=8000, reload=True)
