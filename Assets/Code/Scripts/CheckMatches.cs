using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct Match {
  public int tileId;
  public int size;
  public Vector3Int location;
  public List<Vector3Int> tiles;
}

public class CheckMatches : MonoBehaviour {

  private Tilemap levelTilemap;
  private TurnCounter turnCounter;
  private LevelController levelController;
  private GoalText goalText;


  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    turnCounter = GameObject.FindGameObjectWithTag("TurnCounter").GetComponent<TurnCounter>();
    levelController = gameObject.GetComponent<LevelController>();
  }

  // For debugging
  void PrintMatch(Match match) {
    Debug.Log("Tile id: " + match.tileId + " Size: " + match.size + " Location: " + match.location);
    Debug.Log("Tiles: ");
    foreach (Vector3Int tile in match.tiles) {
      Debug.Log(tile);
    }
  }

  // This bad boy got hands
  // Flood fills all tiles in level tilemap and returns a list of matches
  // Keeps track of visited tiles so should be unique matches
  public List<Match> GetAllMatches() {
    Stack<Vector3Int> tiles = new Stack<Vector3Int>();
    List<Vector3Int> visited = new List<Vector3Int>();
    List<Match> matches = new List<Match>();

    foreach (Vector3Int position in levelTilemap.cellBounds.allPositionsWithin) {

      if (levelTilemap.HasTile(position)) {
        if (!visited.Contains(position) && !tiles.Contains(position)) {
          tiles.Push(position);
          Match match = new Match();
          match.location = position;
          match.tiles = new List<Vector3Int>();
          while (tiles.Count > 0) {
            Vector3Int tilePos = tiles.Pop();
            if (levelTilemap.HasTile(tilePos) && !tiles.Contains(tilePos)) {
              GameTile tile = levelTilemap.GetTile<GameTile>(tilePos);
              if (match.tileId == 0) {
                match.tileId = tile.id;
              }
              if (tile.id == match.tileId) {
                match.size++;
                match.tiles.Add(tilePos);
                visited.Add(tilePos);

                Vector3Int east = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
                Vector3Int west = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
                Vector3Int south = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);
                Vector3Int north = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
                if (!visited.Contains(east))
                  tiles.Push(east);
                if (!visited.Contains(west))
                  tiles.Push(west);
                if (!visited.Contains(north))
                  tiles.Push(north);
                if (!visited.Contains(south))
                  tiles.Push(south);
              }
            }
          }
          matches.Add(match);
        }
      }
    }
    return matches;
  }

  List<Vector3Int> GetMatchShape(Match match) {
    Vector3Int offset = match.tiles[0];
    List<Vector3Int> shape = new List<Vector3Int>();
    foreach (Vector3Int tile in match.tiles) {
      shape.Add(tile - offset);
    }
    return shape;
  }

  bool ShapesAreEqual(List<Vector3Int> shape1, List<Vector3Int> shape2) {
    if (shape1.Count != shape2.Count) return false;
    for (int i = 0; i < shape1.Count; i++) {
      if (shape1[i] != shape2[i]) return false;
    }
    return true;
  }

  public List<Match> CheckMatchShapes(List<Match> matches) {
    List<Match> validMatches = new List<Match>();

    foreach (Match match in matches) {
      if (match.size > 3) {
        validMatches.Add(match);
        continue;
      }

      List<Vector3Int> shape = GetMatchShape(match);
    }
    return validMatches;
  }
}
