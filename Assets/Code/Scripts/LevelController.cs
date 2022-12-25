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
  private CheckMatches checkMatches;
  private GoalText goalText;
  private DestroyMatches destroyMatches;
  private FallingTiles fallingTiles;
  private SpawnTiles spawnTiles;
  private BlackHole blackHole;
  private Goal goal;
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

    checkMatches = gameObject.GetComponent<CheckMatches>();

    destroyMatches = gameObject.GetComponent<DestroyMatches>();
    fallingTiles = gameObject.GetComponent<FallingTiles>();
    spawnTiles = gameObject.GetComponent<SpawnTiles>();
    blackHole = gameObject.GetComponent<BlackHole>();

    goal = gameObject.GetComponent<Goal>();
    goalId = goal.goalId;
  }

  private void takeTurn() {
    turnsRemaining--;
    turnCounter.updateText(turnsRemaining);
  }

  public void OnSwitch(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    takeTurn();


    StartCoroutine(DestroyTilesLoop(draggedTilePosition, targetTilePosition));
    if (turnsRemaining == 0) {
      // End game
    }
  }

  private IEnumerator HandlePowerup(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    GameTile draggedTile = levelTilemap.GetTile<GameTile>(draggedTilePosition);
    GameTile targetTile = levelTilemap.GetTile<GameTile>(targetTilePosition);

    if (draggedTile.type == GameTile.Type.Power || targetTile.type == GameTile.Type.Power) {
      GameTile powerUp = draggedTile.type == GameTile.Type.Power ? draggedTile : targetTile;
      GameTile switchedTile = draggedTile.type != GameTile.Type.Power ? draggedTile : targetTile;
      Vector3Int position = draggedTile.type == GameTile.Type.Power ? draggedTilePosition : targetTilePosition;
      yield return StartCoroutine(blackHole.SpawnBlackHole(position, switchedTile));
    }
  }

  private IEnumerator DestroyTilesLoop(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    List<Match> matches = checkMatches.GetAllMatches();
    matches = checkMatches.CheckMatchShapes(matches);
    yield return HandlePowerup(draggedTilePosition, targetTilePosition);
    do {
      goal.goalCompletion += goal.GetGoalCompletion(matches);
      goal.goalText.UpdateText(goal.goalCompletion, goal.goalTarget);
      // TODO: Scoring here
      destroyMatches.DestroyTiles(matches);
      // Wait for animation to finish
      yield return new WaitForSeconds(0.3f); // TODO: Switch to waiting for animation length
      yield return StartCoroutine(fallingTiles.CheckTiles());
      spawnTiles.SpawnRandomTilesToFill();
      yield return new WaitForSeconds(0.3f);
      matches = checkMatches.GetAllMatches();
      matches = checkMatches.CheckMatchShapes(matches);
    } while (matches.Count > 0);
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

