using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnPowerups : MonoBehaviour {

  private SpawnTiles spawnTiles;
  [SerializeField] GameTile[] powerUpTiles;

  void Start() {
    spawnTiles = gameObject.GetComponent<SpawnTiles>();
  }

  public IEnumerator SpawnPowerupsFromMatches(List<Match> matches) {
    foreach (Match match in matches) {

      if (match.size == 6) {
        int index = Random.Range(0, match.tiles.Count);
        yield return StartCoroutine(spawnTiles.SpawnTile(match.tiles[index], powerUpTiles[0]));
      }
      if (match.size == 7) {
        int index = Random.Range(0, match.tiles.Count);
        yield return StartCoroutine(spawnTiles.SpawnTile(match.tiles[index], powerUpTiles[1]));
      }
      if (match.size >= 8) {
        int index = Random.Range(0, match.tiles.Count);
        yield return StartCoroutine(spawnTiles.SpawnTile(match.tiles[index], powerUpTiles[2]));
      }
    }
  }
}
