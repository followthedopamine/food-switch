using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Goal : MonoBehaviour {

  public int goalId;

  private void Start() {
    Tilemap goalTilemap = GameObject.FindGameObjectWithTag("Goal").GetComponent<Tilemap>();
    GameTile goalTile = GetFirstTile(goalTilemap);
    goalId = goalTile.id;

  }

  private GameTile GetFirstTile(Tilemap tilemap) {
    foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin) {
      if (tilemap.HasTile(position)) {
        Debug.Log(tilemap.GetTile(position));
        GameTile tile = tilemap.GetTile<GameTile>(position);
        return tile;
      }
    }
    return new GameTile();
  }
}
