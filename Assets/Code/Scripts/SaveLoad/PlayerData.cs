using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
  public int tutorialProgress = 0;
  public int[] levelCompletion;

  public PlayerData() {
    tutorialProgress = GameController.Instance.tutorialProgress;
    levelCompletion = GameController.Instance.levelCompletion;
  }
}
