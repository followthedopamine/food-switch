using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BlackHole : MonoBehaviour {

  private Tilemap levelTilemap;
  [SerializeField] private GameObject blackHolePrefab;
  private GameObject blackHole;
  private float scaleSpeed = 2f;
  private float suckSpeed = 6f;
  private float rotationSpeed = 2f;
  private Score score;
  private Goal goal;
  private GameObject levelObject;

  private void Start() {
    levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelTilemap = levelObject.GetComponent<Tilemap>();
    score = gameObject.GetComponent<Score>();
    goal = gameObject.GetComponent<Goal>();
  }

  public IEnumerator SpawnBlackHole(Vector3Int position, GameTile tile) {
    Vector3 worldPosition = levelTilemap.GetCellCenterWorld(position);
    blackHole = Instantiate(blackHolePrefab);
    blackHole.transform.position = worldPosition;
    blackHole.transform.localScale = new Vector3(0.1f, 0.1f, 0);
    Transform gameGrid = levelObject.transform.parent;
    float numberOfTiles = 2.5f;
    levelTilemap.SetTile(position, null);

    while (blackHole.transform.localScale != gameGrid.localScale * numberOfTiles) {
      blackHole.transform.localScale = Vector3.MoveTowards(blackHole.transform.localScale, gameGrid.localScale * numberOfTiles, scaleSpeed * Time.deltaTime);
      yield return new WaitForEndOfFrame();
    }
    yield return StartCoroutine(SuckTiles(position, tile));

    while (blackHole.transform.localScale != new Vector3(0.1f, 0.1f, 0)) {
      blackHole.transform.localScale = Vector3.MoveTowards(blackHole.transform.localScale, new Vector3(0.1f, 0.1f, 0), scaleSpeed * Time.deltaTime);
      yield return new WaitForEndOfFrame();
    }
    Destroy(blackHole);

  }



  private IEnumerator SuckTiles(Vector3Int position, GameTile tile) {
    List<Vector3Int> tiles = TileUtil.FindAllTilesOfType(levelTilemap, tile);
    Vector3 targetPosition = levelTilemap.GetCellCenterWorld(position);
    foreach (Vector3Int tilePosition in tiles) {
      GameObject newTile = TileUtil.ReplaceTileWithGameObject(levelTilemap, tilePosition);
      while (newTile.transform.position != targetPosition) {
        newTile.transform.position = Vector3.MoveTowards(newTile.transform.position, targetPosition, suckSpeed * Time.deltaTime);
        //newTile.transform.rotation.x + Time.deltaTime * rotationSpeed
        newTile.transform.localScale = Vector3.MoveTowards(newTile.transform.localScale, Vector3.zero, Time.deltaTime);
        newTile.transform.Rotate(0, 0, rotationSpeed, Space.Self);
        yield return new WaitForEndOfFrame();
      }
      Destroy(newTile);
      score.AddScore(10);
      if (goal.goalId == tile.id) goal.goalCompletion++;
    }
  }

  // Shake camera maybe?
}
