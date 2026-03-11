# Sports Performance Analysis and Injury Prevention Platform
## Overview

This project is a web-based platform designed to analyze athletes' performance data and help prevent injuries using data analysis and artificial intelligence techniques. The application collects physiological and activity-related data from athletes (either manually entered or imported from wearable devices) and processes it to provide insights about training load, recovery, and potential injury risks.

The system combines modern web technologies with machine learning models to assist athletes and coaches in optimizing training routines and monitoring physical condition.

The goal of the platform is to:

Analyze athlete performance metrics

Predict physiological indicators using machine learning

Identify potential risk factors related to fatigue or overtraining

Support better decision-making for training optimization and injury prevention

The idea of the platform is based on collecting performance-related data from wearable devices such as smart watches or fitness trackers, or through manual input provided by the user. 



## System Architecture

The project is composed of multiple components:

1. Web Application

The web platform allows users to interact with the system and visualize their performance data.

Technologies used:

ASP.NET Core / .NET

Angular

SignalR (for real-time updates)

SQL Server (SSMS) for data storage

Main features include:

User data input and management

Activity and physiological data tracking

Visualization of performance metrics

Integration with the AI prediction service

Artificial Intelligence Module

The AI component analyzes athlete data and predicts physiological indicators that can help evaluate training intensity and recovery status.

The current model predicts average heart rate during an activity based on several physiological and contextual parameters.

## Input Features

The model uses the following attributes:

Age

Gender

Weight

Height

Resting heart rate

Recovery score

Activity type

Activity duration

The target variable predicted by the model is:

Average heart rate during the activity

The dataset is cleaned, encoded, and normalized before training. 

Modelare_whoopDataset

## Machine Learning Model

Two approaches are implemented:

1. Baseline Model

A Ridge Regression model is used as a baseline to evaluate the performance of the neural network.

2. Neural Network (MLP)

A Multilayer Perceptron (MLP) built with TensorFlow/Keras is used to learn nonlinear relationships between athlete characteristics and physiological responses.

## Network architecture:

Input layer

Dense layer (64 neurons, ReLU)

Dense layer (32 neurons, ReLU)

Output layer (1 neuron – regression output)

The model is trained using:

Adam optimizer

Mean Squared Error (MSE) loss

Mean Absolute Error (MAE) metric

The dataset is split into:

70% training

20% validation

10% test data

The trained model can later be exported and integrated into a microservice for real-time predictions. 

Modelare_whoopDataset

Data Processing Pipeline

The machine learning pipeline includes:

Dataset loading

Data cleaning (removal of missing values)

Feature selection

One-hot encoding for categorical features

Feature scaling using StandardScaler

Model training

Model evaluation

Visualization of results (learning curves and prediction plots)

The model performance is evaluated using:

Mean Squared Error (MSE)

Mean Absolute Error (MAE)

Root Mean Squared Error (RMSE)

Data Visualization

The system generates visualizations such as:

Training vs validation loss curves

Real vs predicted heart rate scatter plots

These visualizations help evaluate the model’s learning behavior and prediction accuracy.

Future Improvements

Possible future improvements include:

Integration with real wearable device APIs

Advanced injury risk prediction models

Additional physiological indicators (HRV, sleep, fatigue)

Personalized training recommendations

Deployment of the AI model as a REST microservice

Implementation of fuzzy logic systems for injury risk evaluation

Project Purpose

This project was developed as part of a Bachelor's thesis focused on applying artificial intelligence in sports performance analysis and injury prevention.

The platform demonstrates how machine learning and modern web technologies can be combined to create intelligent systems that support athletes and coaches in monitoring performance and improving training safety.
