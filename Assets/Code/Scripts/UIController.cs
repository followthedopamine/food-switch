using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
  public static UIController Instance;
  private GameObject endScreen;

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
    endScreen = gameObject.transform.Find("Game Over").gameObject;
    endScreen.SetActive(false);
  }

  public void ShowEndScreen() {
    // TODO: Need to disable switching while this is up probably
    endScreen.SetActive(true);
  }

  public void HideEndScreen() {
    endScreen.SetActive(false);
  }
}
