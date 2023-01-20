using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FreeSwitch : MonoBehaviour {

  private int freeSwitchId = 997;
  private Tilemap levelTilemap;
  private LevelController levelController;
  private DestroyMatches destroyMatches;

  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    levelController = gameObject.GetComponent<LevelController>();
    destroyMatches = gameObject.GetComponent<DestroyMatches>();
  }

  public void HandleFreeSwitch(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    levelController.restoreTurn();
    if (CheckForFreeSwitchTile(draggedTilePosition, targetTilePosition)) {
      Vector3Int freeSwitchLocation = GetFreeSwitchTile(draggedTilePosition, targetTilePosition);
      Match match = new Match();
      match.size = 1;
      match.location = freeSwitchLocation;
      match.tiles = new List<Vector3Int>();
      match.tiles.Add(freeSwitchLocation);
      List<Match> matches = new List<Match>();
      matches.Add(match);
      destroyMatches.DestroyTiles(matches);
    }
  }

  private bool CheckForFreeSwitchTile(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    int draggedTileId = levelTilemap.GetTile<GameTile>(draggedTilePosition).id;
    int targetTileId = levelTilemap.GetTile<GameTile>(targetTilePosition).id;
    if (draggedTileId == freeSwitchId || targetTileId == freeSwitchId) return true;
    return false;
  }

  private Vector3Int GetFreeSwitchTile(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    int draggedTileId = levelTilemap.GetTile<GameTile>(draggedTilePosition).id;
    int targetTileId = levelTilemap.GetTile<GameTile>(targetTilePosition).id;
    if (draggedTileId == freeSwitchId) return draggedTilePosition;
    if (targetTileId == freeSwitchId) return targetTilePosition;
    return new Vector3Int();
  }
}
