using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
  public static UIController Instance;
  private GameObject gameOverScreen;
  private GameObject gameWonScreen;
  private GameObject mainMenuScreen;
  private GameObject levelSelectScreen;
  private GameObject creditsScreen;

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
    gameOverScreen = gameObject.transform.Find("Game Over").gameObject;
    gameWonScreen = gameObject.transform.Find("Game Won").gameObject;
    mainMenuScreen = gameObject.transform.Find("Main Menu").gameObject;
    levelSelectScreen = gameObject.transform.Find("Level Select").gameObject;
    creditsScreen = gameObject.transform.Find("Credits Screen").gameObject;
  }

  public void ShowGameOverScreen() {
    // TODO: Need to disable switching while this is up probably
    DisableAllScreens();
    gameOverScreen.SetActive(true);
  }

  public void HideGameOverScreen() {
    gameOverScreen.SetActive(false);
  }

  public void ShowGameWonScreen() {
    DisableAllScreens();
    gameWonScreen.SetActive(true);
  }

  public void HideGameWonScreen() {
    gameWonScreen.SetActive(false);
  }

  public void ShowMainMenuScreen() {
    DisableAllScreens();
    mainMenuScreen.SetActive(true);
  }

  public void HideMainMenuScreen() {
    mainMenuScreen.SetActive(false);
  }

  public void ShowLevelSelectScreen() {
    DisableAllScreens();
    levelSelectScreen.SetActive(true);
  }

  public void HideLevelSelectScreen() {
    levelSelectScreen.SetActive(false);
  }

  public void ShowCreditsScreen() {
    creditsScreen.SetActive(true);
  }

  public void HideCreditsScreen() {
    creditsScreen.SetActive(false);
  }

  private void DisableAllScreens() {
    HideGameOverScreen();
    HideGameWonScreen();
    HideLevelSelectScreen();
    HideMainMenuScreen();
    HideCreditsScreen();
  }
}
