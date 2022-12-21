using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour {

  public Level level;
  [SerializeField] private GameTile[] validTiles;
  private Tilemap levelTilemap;
  private Tilemap backgroundTilemap;
  private Vector3Int tilemapOffset;

  public struct Level {
    public int width;
    public int height;
    public Vector3Int[,] grid;
  }

  void Start() {
    GameObject background = GameObject.FindGameObjectWithTag("Background");
    backgroundTilemap = background.GetComponent<Tilemap>();
    Vector2Int gameDimensions = GetGameDimensions();
    levelTilemap = gameObject.GetComponent<Tilemap>();
    tilemapOffset = TileUtil.GetTilemapOffset(backgroundTilemap);
    level.width = gameDimensions.x;
    level.height = gameDimensions.y;
    level.grid = BuildLevelGrid();
  }

  private Vector2Int GetGameDimensions() {
    List<Vector3Int> tilePositions = TileUtil.GetTilePositions(backgroundTilemap);
    // Calculate dimensions
    // Add 1 to account for index 0
    int width = Math.Abs(tilePositions[0].x - tilePositions[tilePositions.Count - 1].x) + 1;
    int height = Math.Abs(tilePositions[0].y - tilePositions[tilePositions.Count - 1].y) + 1;
    return new Vector2Int(width, height);
  }

  private GameTile GetGameTile(Vector3Int tilePos, bool useOffset = false) {
    List<Vector3Int> tilePositions = TileUtil.GetTilePositions(levelTilemap);
    if (useOffset) {
      tilePos = new Vector3Int(tilePos.x + tilemapOffset.x, tilePos.y + tilemapOffset.y, tilePos.z);
    }
    GameTile tile = levelTilemap.GetTile<GameTile>(tilePos);
    return tile;
  }

  private Vector3Int[,] BuildLevelGrid() {
    Vector3Int[,] grid = new Vector3Int[level.height, level.width];
    for (int r = 0; r < level.height; r++) {
      for (int c = 0; c < level.width; c++) {
        // Apply tilemap offset when getting tile
        // TODO: Check if tile exists
        Vector3Int position = new Vector3Int(c + tilemapOffset.x, r + tilemapOffset.y, 0);
        grid[r, c] = position;
      }
    }
    return grid;
  }



  // void Update() {
  //   if (Input.GetMouseButtonDown(0)) {
  //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
  //     Vector3Int location = levelTilemap.WorldToCell(mousePos);
  //     GameTile tile = levelTilemap.GetTile<GameTile>(location);
  //   }
  // }
}
