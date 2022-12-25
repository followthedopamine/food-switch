using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUps : MonoBehaviour {
  private Tilemap levelTilemap;
  private BlackHole blackHole;

  private void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    blackHole = gameObject.GetComponent<BlackHole>();
  }

  public IEnumerator HandlePowerup(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    GameTile draggedTile = levelTilemap.GetTile<GameTile>(draggedTilePosition);
    GameTile targetTile = levelTilemap.GetTile<GameTile>(targetTilePosition);

    if (draggedTile.type == GameTile.Type.Power || targetTile.type == GameTile.Type.Power) {
      GameTile powerUp = draggedTile.type == GameTile.Type.Power ? draggedTile : targetTile;
      GameTile switchedTile = draggedTile.type != GameTile.Type.Power ? draggedTile : targetTile;
      Vector3Int position = draggedTile.type == GameTile.Type.Power ? draggedTilePosition : targetTilePosition;
      yield return StartCoroutine(blackHole.SpawnBlackHole(position, switchedTile));
    }
  }
}
