using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyMatches : MonoBehaviour {

  private Tilemap levelTilemap;
  [SerializeField] private GameObject destroyAnimationPrefab;

  void Start() {
    GameObject levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelTilemap = levelObject.GetComponent<Tilemap>();
  }

  private void PlayDestroyAnimation(Vector3Int tilePos) {
    // Get world location
    Vector3 position = levelTilemap.GetCellCenterWorld(tilePos);
    // Spawn destroy particle effect
    GameObject animation = Instantiate(destroyAnimationPrefab);
    animation.transform.position = position;
  }

  public void DestroyTiles(List<Match> matches) {
    bool shouldPlaySound = false;
    foreach (Match match in matches) {
      foreach (Vector3Int tilePos in match.tiles) {
        levelTilemap.SetTile(tilePos, null);
        PlayDestroyAnimation(tilePos);
        shouldPlaySound = true;
      }
    }
    if (shouldPlaySound) SoundController.Instance.PlayMatchSound();
    VibrationController.Instance.GentleVibration();
  }
}
