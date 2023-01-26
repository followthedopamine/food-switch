using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class SceneController : MonoBehaviour {

  public static SceneController Instance;

  //public List<string> levelList { get; private set; } = new List<string>();

  public List<string> levelList = new List<string>() {
    "0000_Small",
    "0001_Medium",
    "0002_Large",
    "0003_JellyMedium",
    "0004_JellyMedium 2",
    "0005_JellySmall",
    "0006_Boulder Small",
    "0007_Boulder Medium",
    "0008_Boulder Medium 1",
    "0009_BJ Large",
  };

  private string firstLevel = "0000_Small";
  private string defaultLevel = "";

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
    PopulateLevelList();
    LoadAudioCore();
    LoadUI();
  }

  void LoadAudioCore() {
    if (SceneManager.GetSceneByName("Audio Core").isLoaded) return;
    SceneManager.LoadScene("Audio Core", LoadSceneMode.Additive);
  }

  void LoadUI() {
    if (SceneManager.GetSceneByName("UI").isLoaded) return;
    SceneManager.LoadScene("UI", LoadSceneMode.Additive);
  }

  public void LoadMainMenu() {
    LoadUI();
    UnloadCurrentLevel();
  }

  void LoadGame() {
    if (SceneManager.GetSceneByName("Game").isLoaded) return;
    SceneManager.LoadScene("Game", LoadSceneMode.Additive);
  }

  void ForceLoadGame() {
    SceneManager.LoadScene("Game", LoadSceneMode.Additive);
  }

  void UnloadGame() {
    if (SceneManager.GetSceneByName("Game").isLoaded) SceneManager.UnloadSceneAsync("Game");
  }

  void ReloadGame() {
    UnloadGame();
    ForceLoadGame();
  }

  void PopulateLevelList() {
    // foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
    //   if (scene.enabled) {
    //     if (Directory.GetParent(scene.path).Name == "Levels") {
    //       levelList.Add(Path.GetFileNameWithoutExtension(scene.path));
    //     }
    //   }
    // }
    levelList.Sort();
  }

  Scene[] GetLoadedScenes() {
    int countLoaded = SceneManager.sceneCount;
    Scene[] loadedScenes = new Scene[countLoaded];
    for (int i = 0; i < countLoaded; i++) {
      loadedScenes[i] = SceneManager.GetSceneAt(i);
    }
    return loadedScenes;
  }

  string GetCurrentLevel() {
    Scene[] loadedScenes = GetLoadedScenes();
    foreach (Scene scene in loadedScenes) {
      if (levelList.Contains(scene.name)) {
        return scene.name;
      }
    }
    return defaultLevel;
  }

  int GetLevelIndex(string levelName) {
    return levelList.FindIndex(a => a == levelName);
  }

  string GetNextLevel() {
    string currentLevel = GetCurrentLevel();
    int currentLevelIndex = levelList.IndexOf(currentLevel);
    // If last level return level 1
    if (currentLevelIndex == levelList.Count - 1) return defaultLevel;
    return levelList[currentLevelIndex + 1];
  }

  void UnloadCurrentLevel() {
    // This won't work if there's no level loaded (which is probably fine)
    string currentLevel = GetCurrentLevel();
    if (currentLevel == defaultLevel) return;
    SceneManager.UnloadSceneAsync(currentLevel);
  }

  public void LoadFirstLevel() {
    // TODO
    SceneManager.LoadScene(firstLevel, LoadSceneMode.Additive);
    LoadGame();
    GameController.Instance.currentLevel = GetLevelIndex(firstLevel);
  }

  public void LoadNextLevel() {
    UnloadCurrentLevel();
    string nextLevel = GetNextLevel();
    SceneManager.LoadScene(nextLevel, LoadSceneMode.Additive);
    ReloadGame();
    GameController.Instance.currentLevel = GetLevelIndex(nextLevel);
  }

  public void LoadLevelByIndex(int index) {
    UnloadCurrentLevel();
    SceneManager.LoadScene(levelList[index], LoadSceneMode.Additive);
    ReloadGame();
    GameController.Instance.currentLevel = index;
  }

  public void ReloadCurrentLevel() {
    string currentLevel = GetCurrentLevel();
    SceneManager.UnloadSceneAsync(currentLevel);
    SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);
    ReloadGame();
  }
}
