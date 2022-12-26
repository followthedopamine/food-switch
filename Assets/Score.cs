using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Score : MonoBehaviour {

  public int currentScore { get; private set; } = 0;
  private Tilemap scoreTilemap;
  private Vector3Int scoreTilePosition;

  void Start() {
    scoreTilemap = GameObject.FindGameObjectWithTag("Score").GetComponent<Tilemap>();
    scoreTilePosition = TileUtil.GetFirstTilePosition(scoreTilemap);
  }
}
