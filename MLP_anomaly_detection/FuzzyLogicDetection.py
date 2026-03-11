import numpy as np

#apartanenta  function 
def triangular(x,a,b,c):
    if x <= a or x >= c:
        return 0.0
    elif x == b:
        return 1.0
    elif x < b:
        return (x - a) / (b - a)
    else:
        return (c - x) / (c - b)    

def trapezoidal(x,a,b,c,d):
    if x <= a or x >= d:
        return 0.0
    elif x >= b and x <= c:
        return 1.0
    elif x > a and x < b:
        return (x - a) / (b - a)
    else:
        return (d - x) / (d - c)
    

# Sistem fuzzi pentru risc

def fuzzy_injury_risk(bpm_pred, bpm_sensor):
    delta_bpm = abs(bpm_pred - bpm_sensor)

    #Fuzzyficare pentru diferenta de bpm
    diff_small = trapezoidal(delta_bpm, 0, 0, 3, 8) # mic
    diff_medium = triangular(delta_bpm, 5, 12, 20) # mediu
    diff_large = trapezoidal(delta_bpm, 15, 22, 40, 40) # mare

    #Fuzzy rol 
    
    risk_low =  diff_small  #if difference is small, risk is low
    risk_medium = diff_medium #if difference is medium, risk is medium
    risk_high = diff_large #if difference is large, risk is high

    #3.Eazy Defuzzyfication(medium ponderate)
    #small risk = 20
    # medium risk = 50
    # high risk = 80
    numerator = (
        risk_low * 20 + 
        risk_medium * 50 + 
        risk_high * 80
    ) 
    denominator = risk_low + risk_medium + risk_high

    if denominator == 0:
        risk_score = 0.0
    else:
        risk_score = numerator / denominator
    
    #Decizea finala 
    if risk_score >= 60:
        decision = "High Risk for Injury"
    else:
        decision = "No Risk for Injury"

    return {
        "bpm_pred": round(float(bpm_pred), 2),
        "bpm_sensor": round(float(bpm_sensor), 2),
        "delta_bpm": round(float(delta_bpm), 2),
        "diff_small": round(float(diff_small), 3),
        "diff_medium": round(float(diff_medium), 3),
        "diff_large": round(float(diff_large), 3),
        "risk_score": round(float(risk_score), 2),
        "decision": decision
    }

rezultat = fuzzy_injury_risk(bpm_pred=142, bpm_sensor=144)
print(rezultat)
