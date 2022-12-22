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
    // TODO: While matches loop
    // Disable input
    // Check matches
    // Destroy matches (play animation)
    // Spawn matches (play animation)
    // Repeat
    StartCoroutine(DestroyTilesLoop());

  }

  private IEnumerator DestroyTilesLoop() {

    List<Match> matches = GetAllMatches();
    while (matches.Count > 0) {
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
  // TODO: Some problems detecting matches still :'(
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
}
