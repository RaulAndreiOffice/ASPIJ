import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
import tensorflow as tf
from sklearn.preprocessing import StandardScaler
from tensorflow import keras
from tensorflow.keras import layers
#Pentru grafic
import matplotlib.pyplot as plt
import joblib


df = pd.read_csv("C:\\Users\\raulb\\Documents\\Licienta\\Set date ai\\dataset.csv")

#Preprocesare date

cols_to_drop = ["ID","Calories Burn", "Dream Weight"]
df = df.drop(columns=cols_to_drop)

#Definesc input si output
y = df["Heart Rate"]
X=df.drop(columns=["Heart Rate"])

#Separ coloanele numerice vs coloane categorice

numeric_cols = ["Actual Weight","Age","Duration","BMI","Exercise Intensity"]
categorical_cols = ["Gender","Exercise","Weather Conditions"]

#One-Hot encoding pentru categori
x_categ = pd.get_dummies(X[categorical_cols],drop_first=False)

#Scalare
x_num =X[numeric_cols]

#Facem vectorul final de intrare
x_all = pd.concat([x_num, x_categ], axis=1)

#Acuma impart datele

x_train,x_temp, y_train, y_temp = train_test_split(x_all, y, test_size=0.3, random_state=42)
#Dupa ce am 70% antrenare ramane 30% test pe care il imparty in 20% validare si 10% test
#10% reprezintă o treime (0.33) din cei 30% rămași
x_val ,x_test, y_val, y_test = train_test_split(x_temp, y_temp,test_size=0.33, random_state=42)

print(f"Dimensiuni finale:")
print(f"Train: {x_train.shape[0]} ( Antrenez )")
print(f"Validare: {x_val.shape[0]} (Verifica in timp real)")
print(f"Test: {x_test.shape[0]} (Verificare finala)")

# REGULA DE AUR: "fit" doar pe Train, "transform" pe toate
scaler = StandardScaler()

#Invatam media si deviatia doar din datele de antrenare

x_train_scaled = scaler.fit_transform(x_train)
# Aplicăm aceeași transformare matematică pe Validare și Test
x_val_scaled = scaler.transform(x_val)
x_test_scaled = scaler.transform(x_test)

input_dim = x_train_scaled.shape[1]
print("Dimensiune vector input:", input_dim)

#Definim modelul MLP
model = keras.Sequential([
    layers.Input(shape=(input_dim,)),
    layers.Dense(64, activation='relu'),
    layers.Dense(32, activation='relu'),
    #layers.Dense(1, activation='relu'),
    layers.Dense(1, activation='linear'),
])

model.compile(
    optimizer=keras.optimizers.Adam(learning_rate= 0.001),
    loss="mse",
    metrics=["mae"]
)

history = model.fit(
    x_train_scaled, y_train,
    validation_data=(x_val_scaled, y_val),
    epochs=50,
    batch_size=32,
    verbose=1
)

model.summary()

# Learning curve (Train vs Validation Loss)
plt.figure(figsize = (10,6))
plt.plot(history.history['loss'], label='Train loss')
plt.plot(history.history['val_loss'], label='Validation loss')
plt.title('Learning Curve: Train vs Validation')
plt.xlabel('Epoci')
plt.ylabel('Eroare ')
plt.legend()
plt.grid(True)
plt.show()

#MSE

y_test_pred = model.predict(x_test_scaled).flatten()
mse = np.mean((y_test.values - y_test_pred) ** 2)
print("MSE pe TEST:", mse)

#MAE
from sklearn.metrics import mean_absolute_error

# Predictii pe test
y_test_pred = model.predict(x_test_scaled).flatten()

# MAE
mae = mean_absolute_error(y_test, y_test_pred)

print("MAE pe TEST:", mae, "bpm")

#Tabel final corect
from sklearn.metrics import mean_squared_error, mean_absolute_error

y_test_pred = model.predict(x_test_scaled).flatten()

mse = mean_squared_error(y_test, y_test_pred)
mae = mean_absolute_error(y_test, y_test_pred)
rmse = np.sqrt(mse)
loss_test = mse   # pentru ca loss = MSE

metrics_table = pd.DataFrame({
    "Metrică": ["LOSS (MSE)", "MSE", "MAE", "RMSE"],
    "Valoare": [loss_test, mse, mae, rmse]
})

print("\n=== Tabel metrici finale ===")
print(metrics_table)






#model.save("hr_model.h5")

#joblib.dump(scaler, "scaler.pkl")

#import pickle
#with open("columns.pkl", "wb") as f:
 #   pickle.dump(x_all.columns.tolist(), f)