using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour {
  public void PlayButton() {
    Debug.Log("Play button pressed");
    UIController.Instance.HideMainMenuScreen();
    // Load level scene
    SceneController.Instance.LoadFirstLevel();
  }

  public void ReloadButton() {
    UIController.Instance.HideAllScreens();
    SceneController.Instance.ReloadCurrentLevel();
  }

  public void HomeButton() {
    UIController.Instance.HideAllScreens();
    SceneController.Instance.LoadMainMenu();
    UIController.Instance.ShowMainMenuScreen();
  }
}
