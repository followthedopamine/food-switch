using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileUtil {

  public static Vector2 ConvertTilemapPosition(Vector3 position) {
    return new Vector2(position.x + 0.5f, position.y + 0.5f);
  }

  public static List<Vector2> GetTilePositionsInWorld(Tilemap tilemap) {
    List<Vector2> tiles = new List<Vector2> { };

    foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin) {
      if (tilemap.HasTile(position)) {
        Vector2 tilePos = tilemap.GetCellCenterWorld(position);//ConvertTilemapPosition(tilemap.GetCellCenterWorld(position));
        tiles.Add(tilePos);
      }
    }
    return tiles;
  }

  public static List<Vector2> GetTilePositions(Tilemap tilemap) {
    List<Vector2> tiles = new List<Vector2> { };

    foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin) {
      if (tilemap.HasTile(position)) {
        Vector2 tilePos = new Vector2(position.x, position.y);
        tiles.Add(tilePos);
      }
    }
    return tiles;
  }
}
