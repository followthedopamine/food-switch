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
    GameTile goalTile = TileUtil.GetFirstTile(goalTilemap);
    GameObject levelObject = GameObject.FindGameObjectWithTag("LevelController");
    goalTarget = levelObject.GetComponent<LevelController>().goalTarget;
    goalId = goalTile.id;
    goalText = GameObject.FindGameObjectWithTag("GoalText").GetComponent<GoalText>();
    goalText.UpdateText(goalCompletion, goalTarget);
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
