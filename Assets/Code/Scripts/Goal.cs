using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Goal : MonoBehaviour {
  [HideInInspector] public GoalText goalText;
  [HideInInspector] public int goalId;
  [HideInInspector] public int goalCompletion;
  public int goalTarget;
  private int jellyTileOffset = 2000;
  private Tilemap levelTilemap;


  private void Start() {
    Tilemap goalTilemap = GameObject.FindGameObjectWithTag("Goal").GetComponent<Tilemap>();
    GameTile goalTile = TileUtil.GetFirstTile(goalTilemap);
    GameObject levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelTilemap = levelObject.GetComponent<Tilemap>();
    goalTarget = levelObject.GetComponent<LevelController>().goalTarget;
    goalId = goalTile.id;
    goalText = GameObject.FindGameObjectWithTag("GoalText").GetComponent<GoalText>();
    goalText.UpdateText(goalCompletion, goalTarget);
  }

  public int GetGoalCompletion(List<Match> matches) {
    int goalCompletion = 0;
    foreach (Match match in matches) {
      if (match.tileId >= jellyTileOffset && match.tileId == goalId) {
        goalCompletion += GetNumberOfJellyTiles(match);
      } else {
        if (match.tileId == goalId) {
          goalCompletion += match.size;
        }
      }

    }
    return goalCompletion;
  }

  private int GetNumberOfJellyTiles(Match match) {
    int jellyTileCount = 0;
    foreach (Vector3Int tilePosition in match.tiles) {
      if (levelTilemap.GetTile<GameTile>(tilePosition).id >= jellyTileOffset) {
        jellyTileCount++;
      }
    }
    return jellyTileCount;
  }
}
