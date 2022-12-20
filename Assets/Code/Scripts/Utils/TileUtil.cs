using System;
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

  // Get tilemap distance from 0 in number of tiles
  public static Vector3Int GetTilemapOffset(Tilemap tilemap) {
    Vector3Int tile = new Vector3Int(0, 0, 0);
    foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin) {
      if (tilemap.HasTile(position)) {
        tile = position;
        break;
      }
    }
    return tile;
  }

  // setPos0 = true if you wish to have positions returned with top left 0, 0
  public static List<Vector3Int> GetTilePositions(Tilemap tilemap) {
    List<Vector3Int> tiles = new List<Vector3Int> { };
    foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin) {
      if (tilemap.HasTile(position)) {
        tiles.Add(position);
      }
    }
    return tiles;
  }
}
