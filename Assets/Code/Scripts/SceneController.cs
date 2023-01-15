using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class SceneController : MonoBehaviour {

  public static SceneController Instance;

  public List<string> levelList { get; private set; } = new List<string>();

  private string defaultLevel = "0000_Test";

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
    LoadGame();
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

  void LoadLevel(int levelNumber) {

  }

  void PopulateLevelList() {
    foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
      if (scene.enabled) {
        if (Directory.GetParent(scene.path).Name == "Levels") {
          levelList.Add(Path.GetFileNameWithoutExtension(scene.path));
          Debug.Log(Path.GetFileNameWithoutExtension(scene.path));
        }
      }
    }
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
    SceneManager.UnloadSceneAsync(currentLevel);
  }

  public void LoadFirstLevel() {
    // TODO
    SceneManager.LoadScene(defaultLevel, LoadSceneMode.Additive);
  }

  public void LoadNextLevel() {
    UnloadCurrentLevel();
    string nextLevel = GetNextLevel();
    SceneManager.LoadScene(nextLevel, LoadSceneMode.Additive);
  }

  public void ReloadCurrentLevel() {
    string currentLevel = GetCurrentLevel();
    SceneManager.UnloadSceneAsync(currentLevel);
    SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);
  }
}
