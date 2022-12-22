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
  private DestroyMatches destroyMatches;
  private SpawnTiles spawnTiles;
  private FallingTiles fallingTiles;

  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    destroyMatches = gameObject.GetComponent<DestroyMatches>();
    spawnTiles = gameObject.GetComponent<SpawnTiles>();
    fallingTiles = gameObject.GetComponent<FallingTiles>();
  }

  public void OnSwitch() {
    StartCoroutine(DestroyTilesLoop());
  }

  private IEnumerator DestroyTilesLoop() {
    List<Match> matches = GetAllMatches();

    while (matches.Count > 0) {
      matches = CheckMatchShapes(matches);
      destroyMatches.DestroyTiles(matches);
      // Wait for animation to finish
      yield return new WaitForSeconds(0.3f); // TODO: Switch to waiting for animation length
      fallingTiles.CheckTiles();
      yield return new WaitForSeconds(0.3f);
      spawnTiles.SpawnRandomTilesToFill();
      matches = GetAllMatches();
    }
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
  List<Match> GetAllMatches() {
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

  List<Match> CheckMatchShapes(List<Match> matches) {

    // List<List<Vector3Int>> validShapes = new List<List<Vector3Int>>() {

    //   // 3 tiles in a row [][][]
    //   new List<Vector3Int>() {
    //     new Vector3Int(0, 0, 0),
    //     new Vector3Int(1, 0, 0),
    //     new Vector3Int(2, 0, 0)
    //   },

    //   // 3 tiles in a col [][][]
    //   new List<Vector3Int>() {
    //     new Vector3Int(0, 0, 0),
    //     new Vector3Int(0, 1, 0),
    //     new Vector3Int(0, 2, 0)
    //   },

    //   // 4 tiles in a row [][][]
    //   new List<Vector3Int>() {
    //     new Vector3Int(0, 0, 0),
    //     new Vector3Int(1, 0, 0),
    //     new Vector3Int(2, 0, 0),
    //     new Vector3Int(3, 0, 0),
    //   },

    //   // 4 tiles in a col [][][]
    //   new List<Vector3Int>() {
    //     new Vector3Int(0, 0, 0),
    //     new Vector3Int(0, 1, 0),
    //     new Vector3Int(0, 2, 0),
    //     new Vector3Int(0, 3, 0),
    //   },

    //   // 5 tiles in a row [][][]
    //   new List<Vector3Int>() {
    //     new Vector3Int(0, 0, 0),
    //     new Vector3Int(1, 0, 0),
    //     new Vector3Int(2, 0, 0),
    //     new Vector3Int(3, 0, 0),
    //     new Vector3Int(4, 0, 0),
    //   },

    //   // 5 tiles in a col [][][]
    //   new List<Vector3Int>() {
    //     new Vector3Int(0, 0, 0),
    //     new Vector3Int(0, 1, 0),
    //     new Vector3Int(0, 2, 0),
    //     new Vector3Int(0, 3, 0),
    //     new Vector3Int(0, 4, 0),
    //   },


    // };

    List<Match> validMatches = new List<Match>();

    foreach (Match match in matches) {
      if (match.size > 3) {
        validMatches.Add(match);
        continue;
      }

      List<Vector3Int> shape = GetMatchShape(match);
      // foreach (List<Vector3Int> validShape in validShapes) {
      //   if (ShapesAreEqual(shape, validShape)) {
      //     validMatches.Add(match);
      //   }
      // }
    }
    return validMatches;
  }
}
