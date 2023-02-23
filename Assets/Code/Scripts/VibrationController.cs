using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDG;

public class VibrationController : MonoBehaviour {
  public static VibrationController Instance;

  public bool vibrationEnabled;

  void Awake() {
    if (Instance != null) {
      if (Instance != this) {
        Destroy(this);
      }
    } else {
      Instance = this;
    }
  }

  void Start() {
    vibrationEnabled = PlayerPrefs.GetInt("Vibration", 1) > 0;
  }

  public void UpdateVibration(bool isEnabled) {
    vibrationEnabled = isEnabled;
    int isEnabledInt = isEnabled ? 1 : 0;
    PlayerPrefs.SetInt("Vibration", isEnabledInt);
  }

  bool CheckShouldVibrate() {
    // Check if haptics enabled
    if (!vibrationEnabled) return false;
    // Check if platform is Android
    if (Application.platform != RuntimePlatform.Android) return false;
    return true;
  }

  public void GentleVibration() {
    if (!CheckShouldVibrate()) return;
    Vibration.Vibrate(100, 50, true);
  }

  public void HardVibration() {
    if (!CheckShouldVibrate()) return;
    Vibration.Vibrate(100, 255, true);
  }

  public void SuccessVibration() {
    if (!CheckShouldVibrate()) return;
    Vibration.Vibrate(200, 100, true);
  }
}
