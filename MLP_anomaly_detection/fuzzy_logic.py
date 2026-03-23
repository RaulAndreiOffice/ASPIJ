"""
Fuzzy Logic Interpretation Layer
---------------------------------
Input:  heart_rate_predicted, difference (HR delta), is_anomaly
Output: effort_level, fatigue_risk, recommendation

Pipeline: AI model (MLP) → THIS MODULE → Backend (.NET)
DO NOT mix ML prediction logic here.
"""

from dataclasses import dataclass
from typing import Optional


@dataclass
class FuzzyResult:
    effort_level: str        # low | moderate | high | extreme
    fatigue_risk: str        # low | medium | high
    recommendation: str      # one of four fixed strings


# ---------------------------------------------------------------------------
# Membership helpers  (triangular / trapezoidal, returns 0.0–1.0)
# ---------------------------------------------------------------------------

def _trapezoid(x: float, a: float, b: float, c: float, d: float) -> float:
    """Trapezoidal membership: rise a→b, flat b→c, fall c→d."""
    if x <= a or x >= d:
        return 0.0
    if b <= x <= c:
        return 1.0
    if x < b:
        return (x - a) / (b - a)
    return (d - x) / (d - c)


def _triangle(x: float, a: float, b: float, c: float) -> float:
    """Triangular membership: rise a→b, fall b→c."""
    if x <= a or x >= c:
        return 0.0
    if x <= b:
        return (x - a) / (b - a)
    return (c - x) / (c - b)


# ---------------------------------------------------------------------------
# Heart-rate universe membership functions
# ---------------------------------------------------------------------------

def _hr_low(hr: float) -> float:
    return _trapezoid(hr, 40, 40, 110, 130)

def _hr_moderate(hr: float) -> float:
    return _trapezoid(hr, 110, 120, 145, 155)

def _hr_high(hr: float) -> float:
    return _trapezoid(hr, 145, 155, 165, 175)

def _hr_extreme(hr: float) -> float:
    return _trapezoid(hr, 165, 175, 220, 220)


# ---------------------------------------------------------------------------
# Difference (|measured − predicted|) membership functions
# ---------------------------------------------------------------------------

def _diff_small(d: float) -> float:
    return _trapezoid(d, 0, 0, 10, 20)

def _diff_medium(d: float) -> float:
    return _triangle(d, 10, 20, 35)

def _diff_large(d: float) -> float:
    return _trapezoid(d, 25, 35, 200, 200)


# ---------------------------------------------------------------------------
# Core fuzzy engine
# ---------------------------------------------------------------------------

def evaluate(
    heart_rate_predicted: float,
    difference: Optional[float],
    is_anomaly: Optional[bool],
) -> FuzzyResult:
    """
    Apply fuzzy rules and return crisp categorical outputs.

    Parameters
    ----------
    heart_rate_predicted : float
        Predicted HR from MLP model.
    difference : float | None
        |HeartRate_Measured − heart_rate_predicted|. None when no measurement.
    is_anomaly : bool | None
        Anomaly flag from service. None when no measurement.
    """

    # --- effort level (HR-based) -------------------------------------------
    mu_low      = _hr_low(heart_rate_predicted)
    mu_moderate = _hr_moderate(heart_rate_predicted)
    mu_high     = _hr_high(heart_rate_predicted)
    mu_extreme  = _hr_extreme(heart_rate_predicted)

    effort_scores = {
        "low":      mu_low,
        "moderate": mu_moderate,
        "high":     mu_high,
        "extreme":  mu_extreme,
    }
    effort_level = max(effort_scores, key=effort_scores.get)

    # --- fatigue risk (HR + difference + anomaly) ---------------------------
    # When no measurement is available, base risk purely on predicted HR.
    if difference is not None:
        d_small  = _diff_small(difference)
        d_medium = _diff_medium(difference)
        d_large  = _diff_large(difference)
    else:
        # Conservative defaults: treat as small difference
        d_small, d_medium, d_large = 1.0, 0.0, 0.0

    anomaly_boost = 1.0 if is_anomaly else 0.0

    # Fuzzy rules → activation strengths for LOW / MEDIUM / HIGH fatigue
    # R1: low HR  + small diff          → low fatigue
    r1 = min(mu_low, d_small)
    # R2: moderate HR + small diff      → low fatigue
    r2 = min(mu_moderate, d_small)
    # R3: moderate HR + medium diff     → medium fatigue
    r3 = min(mu_moderate, d_medium)
    # R4: high HR + small/medium diff   → medium fatigue
    r4 = min(mu_high, max(d_small, d_medium))
    # R5: high HR + large diff          → high fatigue
    r5 = min(mu_high, d_large)
    # R6: extreme HR (any diff)         → high fatigue
    r6 = mu_extreme
    # R7: anomaly detected + large diff → high fatigue
    r7 = min(anomaly_boost, d_large)
    # R8: anomaly detected + medium diff → medium fatigue
    r8 = min(anomaly_boost, d_medium)
    # R9: low HR + large diff (sensor?)  → medium fatigue
    r9 = min(mu_low, d_large)

    fatigue_low    = max(r1, r2)
    fatigue_medium = max(r3, r4, r8, r9)
    fatigue_high   = max(r5, r6, r7)

    fatigue_scores = {
        "low":    fatigue_low,
        "medium": fatigue_medium,
        "high":   fatigue_high,
    }
    fatigue_risk = max(fatigue_scores, key=fatigue_scores.get)

    # --- recommendation (fatigue + anomaly) ---------------------------------
    if fatigue_risk == "high" and is_anomaly:
        recommendation = "High risk - stop training"
    elif fatigue_risk == "high":
        recommendation = "Rest recommended"
    elif fatigue_risk == "medium":
        recommendation = "Reduce intensity"
    else:
        recommendation = "Continue training"

    return FuzzyResult(
        effort_level=effort_level,
        fatigue_risk=fatigue_risk,
        recommendation=recommendation,
    )
