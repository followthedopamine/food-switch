using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyMatches : MonoBehaviour {

  private Tilemap levelTilemap;

  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
  }

  public void DestroyTiles(List<Match> matches) {
    // TODO: Destroy sub tiles
    // TODO: This desperately needs animation
    foreach (Match match in matches) {
      if (match.size >= 3) {
        foreach (Vector3Int tilePos in match.tiles) {
          levelTilemap.SetTile(tilePos, null);
        }
      }
    }
  }
}
