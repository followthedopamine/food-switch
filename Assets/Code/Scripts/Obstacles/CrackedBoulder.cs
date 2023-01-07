using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrackedBoulder : MonoBehaviour {
  private Tilemap levelTilemap;
  private int crackedBoulderId = 1000;
  private int boulderId = 1001;
  [SerializeField] private GameTile crackedBoulderTile;

  private void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
  }

  private Match CheckMatchForSurroundingBoulders(Match match) {
    Match boulders = new Match();
    boulders.tileId = crackedBoulderId;
    boulders.tiles = new List<Vector3Int>();

    foreach (Vector3Int matchTile in match.tiles) {
      Vector3Int north = new Vector3Int(matchTile.x, matchTile.y - 1, matchTile.z);
      Vector3Int south = new Vector3Int(matchTile.x, matchTile.y + 1, matchTile.z);
      Vector3Int east = new Vector3Int(matchTile.x - 1, matchTile.y, matchTile.z);
      Vector3Int west = new Vector3Int(matchTile.x + 1, matchTile.y, matchTile.z);
      List<Vector3Int> directions = new List<Vector3Int> { north, south, east, west };
      foreach (Vector3Int direction in directions) {
        GameTile tile = levelTilemap.GetTile<GameTile>(direction);
        if (tile == null) continue;
        if (tile.id == crackedBoulderId) {
          if (boulders.location == null) boulders.location = direction;
          boulders.size++;
          boulders.tiles.Add(direction);
        }
        if (tile.id == boulderId) {
          levelTilemap.SetTile(direction, crackedBoulderTile);
        }
      }
    }
    return boulders;
  }

  public List<Match> CheckAllMatchesForSurroundingBoulders(List<Match> matches) {
    List<Match> boulders = new List<Match>();
    foreach (Match match in matches) {
      boulders.Add(CheckMatchForSurroundingBoulders(match));
    }
    return boulders;
  }
}
