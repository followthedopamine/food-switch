using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUps : MonoBehaviour {
  private Tilemap levelTilemap;
  private BlackHole blackHole;
  private LineClear lineClear;
  private FreeSwitch freeSwitch;
  private int blackHoleId = 999;
  private int lightningId = 998;
  private int freeSwitchId = 997;


  private void Start() {
    GameObject levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelTilemap = levelObject.GetComponent<Tilemap>();
    blackHole = gameObject.GetComponent<BlackHole>();
    lineClear = gameObject.GetComponent<LineClear>();
    freeSwitch = gameObject.GetComponent<FreeSwitch>();
  }

  public IEnumerator HandlePowerup(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    GameTile draggedTile = levelTilemap.GetTile<GameTile>(draggedTilePosition);
    GameTile targetTile = levelTilemap.GetTile<GameTile>(targetTilePosition);

    if (draggedTile.type == GameTile.Type.Power || targetTile.type == GameTile.Type.Power) {
      VibrationController.Instance.HardVibration();
      GameTile powerUp = draggedTile.type == GameTile.Type.Power ? draggedTile : targetTile;
      GameTile switchedTile = draggedTile.type != GameTile.Type.Power ? draggedTile : targetTile;
      Vector3Int position = draggedTile.type == GameTile.Type.Power ? draggedTilePosition : targetTilePosition;
      Vector3Int oldPosition = draggedTile.type == GameTile.Type.Power ? targetTilePosition : draggedTilePosition;
      if (powerUp.id == lightningId) {
        yield return StartCoroutine(lineClear.ClearLine(position, oldPosition));
      }
      if (powerUp.id == blackHoleId) {
        yield return StartCoroutine(blackHole.SpawnBlackHole(position, switchedTile));
      }
      if (powerUp.id == freeSwitchId) {
        freeSwitch.HandleFreeSwitch(draggedTilePosition, targetTilePosition);
      }

    }
  }
}
