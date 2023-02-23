using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour {

  [SerializeField] private List<GameObject> musicSlider;
  [SerializeField] private List<GameObject> sfxSlider;
  [SerializeField] private List<GameObject> vibrationToggles;
  [SerializeField] private GameObject nextLevelButton;
  private Button nextLevelButtonComponent;

  void Start() {
    if (Application.platform != RuntimePlatform.Android) {
      for (int i = 0; i < vibrationToggles.Count; i++) {
        vibrationToggles[i].SetActive(false);
      }
    }
  }

  // TODO: Move get component out of this function (it doesn't work in start for some reason)
  void Update() {
    if (GameController.Instance.currentLevel == SceneController.Instance.levelList.Count - 1) {
      nextLevelButtonComponent = nextLevelButton.GetComponent<Button>();
      nextLevelButtonComponent.interactable = false;
    }
  }

  public void UpdateSliders() {
    for (int i = 0; i < musicSlider.Count; i++) {
      sfxSlider[i].GetComponent<Slider>().value = SoundController.Instance.sfxVolumePercentage;
      musicSlider[i].GetComponent<Slider>().value = SoundController.Instance.musicVolumePercentage;
      vibrationToggles[i].GetComponent<Toggle>().isOn = VibrationController.Instance.vibrationEnabled;
    }
  }

  public void PlayButton() {
    UIController.Instance.HideMainMenuScreen();
    // Load level scene
    SceneController.Instance.LoadFirstLevel();
    SoundController.Instance.PlayGameStartSound();
    SoundController.Instance.PlayRandomMusicTrack();
  }

  public void ReloadButton() {
    UIController.Instance.HideAllScreens();
    SceneController.Instance.ReloadCurrentLevel();
  }

  public void HomeButton() {
    SceneController.Instance.LoadMainMenu();
    UIController.Instance.HideAllScreens();
    UIController.Instance.ShowMainMenuScreen();
  }

  public void CreditsButton() {
    UIController.Instance.HideMainMenuScreen();
    UIController.Instance.ShowCreditsScreen();
  }

  public void NextLevelButton() {
    SceneController.Instance.LoadNextLevel();
    UIController.Instance.HideAllScreens();
    SoundController.Instance.PlayGameStartSound();
    SoundController.Instance.PlayRandomMusicTrack();
  }

  public void LevelSelectButton() {
    UIController.Instance.HideMainMenuScreen();
    UIController.Instance.ShowLevelSelectScreen();
  }

  public void LevelButton(int level) {
    SceneController.Instance.LoadLevelByIndex(level);
    UIController.Instance.HideLevelSelectScreen();
    SoundController.Instance.PlayGameStartSound();
    SoundController.Instance.PlayRandomMusicTrack();
  }

  public void OptionsButton() {
    UIController.Instance.ShowOptionsScreen();
  }

  public void ExitGameMenuButton() {
    UIController.Instance.HideInGameMenu();
  }

  public void UpdateMusicVolume(float volume) {
    SoundController.Instance.UpdateMusicVolume(volume);
  }

  public void UpdateSFXVolume(float volume) {
    SoundController.Instance.UpdateSFXVolume(volume);
  }

  public void UpdateVibration(bool isEnabled) {
    VibrationController.Instance.UpdateVibration(isEnabled);
  }
}
