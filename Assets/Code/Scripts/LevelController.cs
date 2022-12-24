using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class LevelController : MonoBehaviour {

  public Level level;
  [SerializeField] private GameTile[] validTiles;
  private Tilemap levelTilemap;
  private Tilemap backgroundTilemap;
  private Vector3Int tilemapOffset;
  [SerializeField] public int turnsRemaining;
  private TurnCounter turnCounter;
  private int goalId;
  [SerializeField] private int goalTarget;
  private CheckMatches checkMatches;
  private int goalCompletion;
  private GoalText goalText;
  private DestroyMatches destroyMatches;
  private FallingTiles fallingTiles;
  private SpawnTiles spawnTiles;
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
    turnCounter = GameObject.FindGameObjectWithTag("TurnCounter").GetComponent<TurnCounter>();
    Tilemap goalTilemap = GameObject.FindGameObjectWithTag("Goal").GetComponent<Tilemap>();
    GameTile goalTile = GetFirstTile(goalTilemap);
    goalId = goalTile.id;
    checkMatches = gameObject.GetComponent<CheckMatches>();
    goalText = GameObject.FindGameObjectWithTag("GoalText").GetComponent<GoalText>();
    goalText.UpdateText(goalCompletion, goalTarget);
    destroyMatches = gameObject.GetComponent<DestroyMatches>();
    fallingTiles = gameObject.GetComponent<FallingTiles>();
    spawnTiles = gameObject.GetComponent<SpawnTiles>();

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

  private void takeTurn() {
    turnsRemaining--;
    turnCounter.updateText(turnsRemaining);
  }

  public void OnSwitch() {
    takeTurn();
    StartCoroutine(DestroyTilesLoop());
    if (turnsRemaining == 0) {
      // End game
    }
  }

  private IEnumerator DestroyTilesLoop() {
    List<Match> matches = checkMatches.GetAllMatches();
    while (matches.Count > 0) {
      matches = checkMatches.CheckMatchShapes(matches);
      goalCompletion += GetGoalCompletion(matches);
      goalText.UpdateText(goalCompletion, goalTarget);
      // TODO: Scoring here
      destroyMatches.DestroyTiles(matches);
      // Wait for animation to finish
      yield return new WaitForSeconds(0.3f); // TODO: Switch to waiting for animation length
      fallingTiles.CheckTiles();
      yield return new WaitForSeconds(1.3f);
      spawnTiles.SpawnRandomTilesToFill();
      matches = checkMatches.GetAllMatches();
    }
  }

  public int GetGoalCompletion(List<Match> matches) {
    int goalCompletion = 0;
    foreach (Match match in matches) {
      if (match.tileId == goalId) {
        goalCompletion += match.size;
      }
    }
    return goalCompletion;
  }

  private Vector2Int GetGameDimensions() {
    List<Vector3Int> tilePositions = TileUtil.GetTilePositions(backgroundTilemap);
    // Calculate dimensions
    // Add 1 to account for index 0
    int width = Math.Abs(tilePositions[0].x - tilePositions[tilePositions.Count - 1].x) + 1;
    int height = Math.Abs(tilePositions[0].y - tilePositions[tilePositions.Count - 1].y) + 1;
    return new Vector2Int(width, height);
  }

  // private GameTile GetGameTile(Vector3Int tilePos, bool useOffset = false) {
  //   List<Vector3Int> tilePositions = TileUtil.GetTilePositions(levelTilemap);
  //   if (useOffset) {
  //     tilePos = new Vector3Int(tilePos.x + tilemapOffset.x, tilePos.y + tilemapOffset.y, tilePos.z);
  //   }
  //   GameTile tile = levelTilemap.GetTile<GameTile>(tilePos);
  //   return tile;
  // }

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
}

