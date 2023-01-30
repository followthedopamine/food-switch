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
  private GameObject optionsScreen;
  private GameObject ingameMenu;

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
    optionsScreen = gameObject.transform.Find("Options Screen").gameObject;
    ingameMenu = gameObject.transform.Find("Game Menu").gameObject;
  }

  public void ShowGameOverScreen() {
    // TODO: Need to disable switching while this is up probably
    HideAllScreens();
    gameOverScreen.SetActive(true);
  }

  public void HideGameOverScreen() {
    gameOverScreen.SetActive(false);
  }

  public void ShowGameWonScreen() {
    HideAllScreens();
    gameWonScreen.SetActive(true);
  }

  public void HideGameWonScreen() {
    gameWonScreen.SetActive(false);
  }

  public void ShowMainMenuScreen() {
    HideAllScreens();
    mainMenuScreen.SetActive(true);
  }

  public void HideMainMenuScreen() {
    mainMenuScreen.SetActive(false);
  }

  public void ShowLevelSelectScreen() {
    HideAllScreens();
    levelSelectScreen.SetActive(true);
  }

  public void HideLevelSelectScreen() {
    levelSelectScreen.SetActive(false);
  }

  public void ShowCreditsScreen() {
    HideAllScreens();
    creditsScreen.SetActive(true);
  }

  public void HideCreditsScreen() {
    creditsScreen.SetActive(false);
  }

  public void ShowOptionsScreen() {
    HideAllScreens();
    optionsScreen.SetActive(true);
  }

  public void HideOptionsScreen() {
    optionsScreen.SetActive(false);
  }

  public void ShowInGameMenu() {
    ingameMenu.SetActive(true);
  }

  public void HideInGameMenu() {
    ingameMenu.SetActive(false);
  }

  public void HideAllScreens() {
    HideGameOverScreen();
    HideGameWonScreen();
    HideLevelSelectScreen();
    HideMainMenuScreen();
    HideCreditsScreen();
    HideOptionsScreen();
    HideInGameMenu();
  }
}
