using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTiles : MonoBehaviour {
  [SerializeField] private GameTile[] spawnableTiles;
  private LevelController levelController;
  private Tilemap levelTilemap;

  void Start() {
    levelController = gameObject.GetComponent<LevelController>();
    levelTilemap = gameObject.GetComponent<Tilemap>();
  }

  private GameTile GetRandomTile() {
    GameTile tile = spawnableTiles[Random.Range(0, spawnableTiles.Length)];
    return tile;
  }

  public void SpawnRandomTilesToFill() {
    Vector3Int[,] grid = levelController.level.grid;
    for (int r = 0; r < grid.GetLength(0); r++) {
      for (int c = 0; c < grid.GetLength(1); c++) {
        levelTilemap.SetTile(grid[r, c], GetRandomTile());
      }
    }
  }
}

