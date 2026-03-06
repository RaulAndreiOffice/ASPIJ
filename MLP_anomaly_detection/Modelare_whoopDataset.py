import pandas as pd
import numpy as np

from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import mean_squared_error, mean_absolute_error
from sklearn.linear_model import Ridge
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers

import matplotlib.pyplot as plt
import joblib
import pickle




df = pd.read_csv("C:\\Users\\raulb\\Documents\\Licienta\\Set date ai\\DateWhoop.csv")

df.columns = [c.strip().lower().replace(" ", "_") for c in df.columns]


# Păstrăm doar coloanele necesare

cols_to_keep = [
    "age",
    "gender",
    "weight_kg",
    "height_cm",
    "resting_heart_rate",
    "recovery_score",
    "activity_type",
    "activity_duration_min",
    "avg_heart_rate"   # TARGET
]
df = df[cols_to_keep].copy()


# 2) Curățare: valori lipsă
# (simplu: scoatem rândurile incomplete)

df = df.dropna()
print("Shape după dropna:", df.shape)

df = df[df["avg_heart_rate"] > 30]

print("Shape după curățare completă:", df.shape)

# 3) Separăm X și y

y = df["avg_heart_rate"]
X = df.drop(columns=["avg_heart_rate"])

numeric_cols = [
    "age", "weight_kg", "height_cm",
    "resting_heart_rate", "recovery_score",
    "activity_duration_min"
]
categorical_cols = ["gender", "activity_type"]


# 4) One-hot pentru categorice
X_categ = pd.get_dummies(X[categorical_cols], drop_first=False)
X_num = X[numeric_cols]

X_all = pd.concat([X_num, X_categ], axis=1)

print("Dimensiune vector input după one-hot:", X_all.shape[1])


# 5) Split train/val/test (70/20/10)

X_train, X_temp, y_train, y_temp = train_test_split(
    X_all, y, test_size=0.30, random_state=42
)

X_val, X_test, y_val, y_test = train_test_split(
    X_temp, y_temp, test_size=0.33, random_state=42
)

print(f"Train: {X_train.shape[0]}, Val: {X_val.shape[0]}, Test: {X_test.shape[0]}")


# 6) Scaling (fit pe train, transform pe rest)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_val_scaled = scaler.transform(X_val)
X_test_scaled = scaler.transform(X_test)

input_dim = X_train_scaled.shape[1]


# 7A) BASELINE: Ridge Regression (foarte bun ca referință)

ridge = Ridge(alpha=1.0, random_state=42)
ridge.fit(X_train_scaled, y_train)

ridge_pred = ridge.predict(X_test_scaled)

ridge_mse = mean_squared_error(y_test, ridge_pred)
ridge_mae = mean_absolute_error(y_test, ridge_pred)
ridge_rmse = np.sqrt(ridge_mse)

print("\n=== Ridge baseline (TEST) ===")
print(f"MSE:  {ridge_mse:.4f}")
print(f"MAE:  {ridge_mae:.4f} bpm")
print(f"RMSE: {ridge_rmse:.4f} bpm")


#  MLP (regresie)

model = keras.Sequential([
    layers.Input(shape=(input_dim,)),
    layers.Dense(64, activation="relu"),
    layers.Dense(32, activation="relu"),
    layers.Dense(1, activation="linear"),
])

model.compile(
    optimizer=keras.optimizers.Adam(learning_rate=0.001),
    loss="mse",
    metrics=["mae"]
)
#Regula de oprire si ajustare
early_stop = EarlyStopping(
    monitor='val_loss',
    patience=12,          # Așteaptă 12 epoci fără îmbunătățire înainte să se oprească
    restore_best_weights=True # Păstrează cel mai bun model, nu ultimul
)

reduce_lr = ReduceLROnPlateau(
    monitor='val_loss',
    factor=0.5,           # Înjumătățește rata de învățare
    patience=5,           # Dacă după 5 epoci loss-ul nu scade
    min_lr=0.00001
)

history = model.fit(
    X_train_scaled, y_train,
    validation_data=(X_val_scaled, y_val),
    epochs=20,
    batch_size=64,
    verbose=1,
    callbacks=[early_stop, reduce_lr]
)

# Learning curve'
import numpy as np

plt.figure(figsize=(10,6))

plt.plot(history.history['loss'][1:], label='Train Loss (MSE)')
plt.plot(history.history['val_loss'][1:], label='Validation Loss (MSE)')

plt.title('Loss Learning Curve')
plt.xlabel('Epoch')
plt.ylabel('Loss (MSE)')
plt.legend()
plt.grid(True)
plt.show()

# Evaluare MLP pe test
y_test_pred = model.predict(X_test_scaled).flatten()

mse = mean_squared_error(y_test, y_test_pred)
mae = mean_absolute_error(y_test, y_test_pred)
rmse = np.sqrt(mse)

print("\n=== MLP (TEST) ===")
print(f"MSE:  {mse:.4f}")
print(f"MAE:  {mae:.4f} bpm")
print(f"RMSE: {rmse:.4f} bpm")

# Scatter real vs prezis
plt.figure(figsize=(8,8))
plt.scatter(y_test, y_test_pred, alpha=0.6)
min_val = min(y_test.min(), y_test_pred.min())
max_val = max(y_test.max(), y_test_pred.max())
plt.plot([min_val, max_val], [min_val, max_val], "r--")
plt.xlabel("avg_heart_rate REAL")
plt.ylabel("avg_heart_rate PREZIS")
plt.title("Real vs Predicted (MLP)")
plt.grid(True)
plt.show()


#8) Salvare artefacte pentru microserviciu

model.save("whoop_avg_hr_model.h5")
joblib.dump(scaler, "whoop_scaler.pkl")

with open("whoop_columns.pkl", "wb") as f:
    pickle.dump(X_all.columns.tolist(), f)

print("\nSalvat: whoop_avg_hr_model.h5, whoop_scaler.pkl, whoop_columns.pkl")
