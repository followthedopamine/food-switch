using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class LevelController : MonoBehaviour {

  public Level level;
  [SerializeField] public GameTile[] spawnableTiles;
  public Tilemap levelTilemap;
  private Tilemap backgroundTilemap;
  public Vector3Int tilemapOffset;
  [SerializeField] public int turnsRemaining;
  private TurnCounter turnCounter;
  private int goalId;
  private CheckMatches checkMatches;
  private GoalText goalText;
  private DestroyMatches destroyMatches;
  private FallingTiles fallingTiles;
  private SpawnTiles spawnTiles;
  private Goal goal;
  private PowerUps powerUps;
  private Score score;
  private CrackedBoulder crackedBoulder;
  private SpawnPowerups spawnPowerups;


  public struct Level {
    public int width;
    public int height;
    public Vector3Int[,] grid;
  }

  void Start() {
    GameObject background = GameObject.FindGameObjectWithTag("Background");
    GameObject gameScripts = GameObject.FindGameObjectWithTag("GameController");
    backgroundTilemap = background.GetComponent<Tilemap>();
    Vector2Int gameDimensions = GetGameDimensions();
    levelTilemap = gameObject.GetComponent<Tilemap>();
    tilemapOffset = TileUtil.GetTilemapOffset(backgroundTilemap);
    level.width = gameDimensions.x;
    level.height = gameDimensions.y;
    level.grid = BuildLevelGrid();

    turnCounter = GameObject.FindGameObjectWithTag("TurnCounter").GetComponent<TurnCounter>();
    checkMatches = gameScripts.GetComponent<CheckMatches>();
    destroyMatches = gameScripts.GetComponent<DestroyMatches>();
    fallingTiles = gameScripts.GetComponent<FallingTiles>();
    spawnTiles = gameScripts.GetComponent<SpawnTiles>();
    goal = gameScripts.GetComponent<Goal>();
    goalId = goal.goalId;
    powerUps = gameScripts.GetComponent<PowerUps>();
    score = gameScripts.GetComponent<Score>();
    crackedBoulder = gameScripts.GetComponent<CrackedBoulder>();
    spawnPowerups = gameScripts.GetComponent<SpawnPowerups>();
  }

  private void takeTurn() {
    turnsRemaining--;
    turnCounter.updateText(turnsRemaining);
  }

  public void restoreTurn() {
    turnsRemaining++;
    turnCounter.updateText(turnsRemaining);
  }

  public void OnSwitch(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    takeTurn();
    StartCoroutine(DestroyTilesLoop(draggedTilePosition, targetTilePosition));
  }

  private IEnumerator DestroyTilesLoop(Vector3Int draggedTilePosition, Vector3Int targetTilePosition) {
    List<Match> matches = checkMatches.GetAllMatches();
    matches = checkMatches.CheckMatchShapes(matches);
    yield return powerUps.HandlePowerup(draggedTilePosition, targetTilePosition);
    do {
      List<Match> boulders = crackedBoulder.CheckAllMatchesForSurroundingBoulders(matches);
      goal.goalCompletion += goal.GetGoalCompletion(matches);
      goal.goalCompletion += goal.GetGoalCompletion(boulders);
      goal.goalText.UpdateText(goal.goalCompletion, goal.goalTarget);
      if (goal.goalCompletion == goal.goalTarget) {
        // Game won
      }
      score.AddScore(checkMatches.GetTotalMatchedTiles(matches) * 10);
      destroyMatches.DestroyTiles(matches);
      destroyMatches.DestroyTiles(boulders);
      yield return StartCoroutine(spawnPowerups.SpawnPowerupsFromMatches(matches));
      yield return new WaitForSeconds(0.3f); // TODO: Switch to waiting for animation length
      yield return StartCoroutine(fallingTiles.CheckTiles());
      spawnTiles.SpawnRandomTilesToFill();
      yield return new WaitForSeconds(0.3f);
      matches = checkMatches.GetAllMatches();
      matches = checkMatches.CheckMatchShapes(matches);
    } while (matches.Count > 0);

    if (goal.goalCompletion > goal.goalTarget) {
      // TODO: Spawn powerups and loop again
      // Then display game won screen
      UIController.Instance.ShowGameWonScreen();
    }

    if (turnsRemaining == 0 && goal.goalCompletion < goal.goalTarget) {
      // Game over
      UIController.Instance.ShowGameOverScreen();
    }
  }

  private Vector2Int GetGameDimensions() {
    List<Vector3Int> tilePositions = TileUtil.GetTilePositions(backgroundTilemap);
    // Calculate dimensions
    // Add 1 to account for index 0
    int width = Math.Abs(tilePositions[0].x - tilePositions[tilePositions.Count - 1].x) + 1;
    int height = Math.Abs(tilePositions[0].y - tilePositions[tilePositions.Count - 1].y) + 1;
    return new Vector2Int(width, height);
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
}

