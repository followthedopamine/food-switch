using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTiles : MonoBehaviour {
  private GameTile[] spawnableTiles;
  private LevelController levelController;
  private Tilemap levelTilemap;
  private float spawnSpeed = 6f;
  GameObject levelObject;

  void Start() {
    levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelController = levelObject.GetComponent<LevelController>();
    levelTilemap = levelObject.GetComponent<Tilemap>();
    spawnableTiles = levelController.spawnableTiles;
  }

  private GameTile GetRandomTile() {
    GameTile tile = spawnableTiles[Random.Range(0, spawnableTiles.Length)];
    return tile;
  }

  private IEnumerator SpawnRandomTile(Vector3Int position) {
    GameTile tile = GetRandomTile();

    yield return StartCoroutine(SpawnTile(position, tile));

  }

  public IEnumerator SpawnTile(Vector3Int position, GameTile tile) {
    Vector3 worldPosition = levelTilemap.GetCellCenterWorld(position);
    Vector3Int[,] grid = levelController.level.grid;
    GameObject newTile = new GameObject();
    // Match game grid scale
    Transform gameGrid = levelObject.transform.parent;
    newTile.transform.localScale = new Vector3(0.1f, 0.1f, 0);//gameGrid.localScale;
    SpriteRenderer spriteRenderer = newTile.AddComponent<SpriteRenderer>();
    spriteRenderer.sprite = tile.sprite;
    newTile.transform.position = worldPosition;
    while (newTile.transform.localScale != gameGrid.localScale) {
      newTile.transform.localScale = Vector3.MoveTowards(newTile.transform.localScale, gameGrid.localScale, spawnSpeed * Time.deltaTime);
      yield return new WaitForEndOfFrame();
    }
    levelTilemap.SetTile(position, tile);
    Destroy(newTile);
  }

  public void SpawnRandomTilesToFill() {
    Vector3Int[,] grid = levelController.level.grid;
    for (int r = 0; r < grid.GetLength(0); r++) {
      for (int c = 0; c < grid.GetLength(1); c++) {
        if (levelTilemap.GetTile(grid[r, c]) == null)
          StartCoroutine(SpawnRandomTile(grid[r, c]));
      }
    }
  }
}

