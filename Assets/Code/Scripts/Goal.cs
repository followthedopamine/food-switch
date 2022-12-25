using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Goal : MonoBehaviour {
  [HideInInspector] public GoalText goalText;
  [HideInInspector] public int goalId;
  [HideInInspector] public int goalCompletion;
  public int goalTarget;



  private void Start() {
    Tilemap goalTilemap = GameObject.FindGameObjectWithTag("Goal").GetComponent<Tilemap>();
    GameTile goalTile = GetFirstTile(goalTilemap);
    goalId = goalTile.id;
    goalText = GameObject.FindGameObjectWithTag("GoalText").GetComponent<GoalText>();
    goalText.UpdateText(goalCompletion, goalTarget);
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

  public int GetGoalCompletion(List<Match> matches) {
    int goalCompletion = 0;
    foreach (Match match in matches) {
      if (match.tileId == goalId) {
        goalCompletion += match.size;
      }
    }
    return goalCompletion;
  }
}
