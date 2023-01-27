using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour {
  public static GameController Instance;

  public int tutorialProgress = 0;
  public int[] levelCompletion = new int[20];
  public int currentLevel = -1;

  void Awake() {
    if (Instance != null) {
      if (Instance != this) {
        Destroy(this);
      }
    } else {
      Instance = this;
      SaveData.LoadPlayerData();
    }
  }

  public void UpdateCurrentLevelCompletion(int trophyLevel) {
    levelCompletion[currentLevel] = trophyLevel;
    SaveData.SavePlayerData();
  }

  public int GetCurrentLevelCompletion() {
    return levelCompletion[currentLevel];
  }
}
