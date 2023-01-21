using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveData {

  static string path = Application.persistentDataPath + "/data.dat";

  public static void SavePlayerData() {
    BinaryFormatter formatter = new BinaryFormatter();

    FileStream stream = new FileStream(path, FileMode.Create);

    PlayerData data = new PlayerData();

    formatter.Serialize(stream, data);
    stream.Close();
  }

  public static void LoadPlayerData() {
    if (File.Exists(path)) {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);

      PlayerData data = formatter.Deserialize(stream) as PlayerData;
      stream.Close();

      UsePlayerData(data);

    } else {
      Debug.LogError("Save file not found in " + path);
      SavePlayerData();
      return;
    }
  }

  public static void UsePlayerData(PlayerData data) {
    GameController.Instance.tutorialProgress = data.tutorialProgress;
    GameController.Instance.levelCompletion = data.levelCompletion;
  }

}
