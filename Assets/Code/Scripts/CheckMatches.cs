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


  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    destroyMatches = gameObject.GetComponent<DestroyMatches>();
    spawnTiles = gameObject.GetComponent<SpawnTiles>();
  }

  public void OnSwitch() {
    List<Match> matches = GetAllMatches();
    destroyMatches.DestroyTiles(matches);
    spawnTiles.SpawnRandomTilesToFill();
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
            visited.Add(tilePos);
            if (levelTilemap.HasTile(tilePos) && !tiles.Contains(tilePos)) {
              GameTile tile = levelTilemap.GetTile<GameTile>(tilePos);
              if (match.tileId == 0) {
                match.tileId = tile.id;
              }
              if (tile.id == match.tileId) {
                match.size++;
                match.tiles.Add(tilePos);

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

  void OnMouseUp() {
    // foreach (Match match in matches) {
    //   PrintMatch(match);
    // }

  }

  void OnMouseDown() {
    List<Vector3Int> list = new List<Vector3Int>();
    list.Add(new Vector3Int(0, 0, 0));
    list.Add(new Vector3Int(1, 1, 0));
    list.Add(new Vector3Int(4, 1, 0));
    if (list.Contains(new Vector3Int(4, 1, 0))) {
      Debug.Log("Trues");
    } else {
      Debug.Log("Falses");
    }
  }
}
