using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnCounter : MonoBehaviour {
  private TMP_Text turnsText;
  private LevelController levelController;

  void Start() {
    turnsText = gameObject.GetComponent<TMP_Text>();
    levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    updateText(levelController.turnsRemaining);
  }

  public void updateText(int turnsRemaining) {
    turnsText.text = "" + turnsRemaining;
  }
}
