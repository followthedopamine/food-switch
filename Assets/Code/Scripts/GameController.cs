using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour {
  public static GameController Instance;

  void Awake() {
    if (Instance != null) {
      if (Instance != this) {
        Destroy(this);
      }
    } else {
      Instance = this;
    }
  }

  void Start() {
    GetGameDimensions();
  }

  private Vector2 GetGameDimensions() {
    // Get background tilemap object
    GameObject background = GameObject.FindGameObjectWithTag("Background");
    Tilemap backgroundTilemap = background.GetComponent<Tilemap>();
    // Detect where top left and bottom right tiles are placed
    List<Vector2> tilePositions = TileUtil.GetTilePositions(backgroundTilemap);
    // Calculate dimensions
    // Add 1 to account for index 0
    float width = Math.Abs(tilePositions[0].x - tilePositions[tilePositions.Count - 1].x) + 1;
    float height = Math.Abs(tilePositions[0].y - tilePositions[tilePositions.Count - 1].y) + 1;

    return new Vector2(width, height);
  }
}
