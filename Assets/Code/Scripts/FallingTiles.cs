using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingTiles : MonoBehaviour {
  // Loop tiles (from bottom?)
  // Check if tile has a tile underneath
  // If it doesn't, remove tile, spawn tile outside of tilemap
  // Shift position down one tile
  // Add to tilemap
  // Repeat

  private LevelController levelController;
  private Tilemap levelTilemap;
  private float fallSpeed = 2f;

  void Start() {
    levelController = gameObject.GetComponent<LevelController>();
    levelTilemap = gameObject.GetComponent<Tilemap>();
  }

  IEnumerator DropTile(Vector3Int current, Vector3Int target) {
    Vector3 position = levelTilemap.GetCellCenterWorld(current);
    Vector3 targetPosition = levelTilemap.GetCellCenterWorld(target);
    GameTile tile = levelTilemap.GetTile<GameTile>(current);
    GameObject newTile = new GameObject();
    // Match game grid scale
    Transform gameGrid = gameObject.transform.parent;
    newTile.transform.localScale = gameGrid.localScale;//gameGrid.localScale;
    SpriteRenderer spriteRenderer = newTile.AddComponent<SpriteRenderer>();
    spriteRenderer.sprite = tile.sprite;
    levelTilemap.SetTile(current, null);
    newTile.transform.position = position;
    while (newTile.transform.position != targetPosition) {
      newTile.transform.position = Vector3.MoveTowards(newTile.transform.position, targetPosition, fallSpeed * Time.deltaTime);
      yield return new WaitForEndOfFrame();
    }
    Destroy(newTile);

    levelTilemap.SetTile(target, tile);
    // yield return new WaitForSeconds(0.3f); // TODO: Change to tile falling time
  }

  bool CheckForNullTiles() {
    Vector3Int[,] grid = levelController.level.grid;

    for (int r = 0; r < grid.GetLength(0); r++) {
      for (int c = 0; c < grid.GetLength(1); c++) {
        if (levelTilemap.GetTile(grid[r, c]) == null) return true;
      }
    }

    return false;
  }

  public void CheckTiles() {
    Vector3Int[,] grid = levelController.level.grid;
    int attempts = levelController.level.height;

    List<List<Vector3Int>> tilesToDrop = new List<List<Vector3Int>>();
    // really dirty way of getting this done
    for (int i = 0; i < attempts; i++) {
      for (int r = 1; r < grid.GetLength(0); r++) {
        for (int c = 0; c < grid.GetLength(1); c++) {
          // if (levelTilemap.GetTile(grid[r, c]) == null)
          // levelTilemap.SetTile(grid[r, c], null);
          if (levelTilemap.GetTile(grid[r, c]) == null) continue;
          if (levelTilemap.GetTile(grid[r - 1, c]) == null) {
            StartCoroutine(DropTile(grid[r, c], grid[r - 1, c]));
          }
        }
      }
    }
  }
}


// Potential function

// for column
// higestfruit
// lowestnull
//  for row
//    if gettile row column == null else 
//      if min lowestnull                  // if max highestfruit
// if lowest null < highest fruit return function not done