using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DragTiles : MonoBehaviour {
  private Vector3 offset;
  private Vector3 screenPoint;
  private Tilemap levelTilemap;
  private Camera cam;
  private Vector3Int draggingFrom;
  private Tile draggedTile;
  private Tile targetTile;
  private LevelController levelController;
  private CheckMatches checkMatches;

  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    cam = Camera.main;
    levelController = gameObject.GetComponent<LevelController>();
    checkMatches = gameObject.GetComponent<CheckMatches>();
  }

  private void OnMouseDown() {
    // Should help with touch input later
    SelectTileForSwitch();
  }

  private void OnMouseUp() {
    SwitchWithSelectedTile();
  }

  private void SelectTileForSwitch() {
    // TODO: Select tile based on direction dragged not on final mouse position
    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    draggingFrom = levelTilemap.WorldToCell(mousePos);
    draggedTile = levelTilemap.GetTile<GameTile>(draggingFrom);
  }

  private void SwitchWithSelectedTile() {
    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    Vector3Int draggingTo = levelTilemap.WorldToCell(mousePos);
    targetTile = levelTilemap.GetTile<GameTile>(draggingTo);
    if (ValidateSwitch(draggingTo, draggingFrom) && targetTile != null) {
      levelTilemap.SetTile(draggingTo, draggedTile);
      levelTilemap.SetTile(draggingFrom, targetTile);
    }
    checkMatches.OnSwitch();
  }

  // This might need to be in a different class
  private bool ValidateSwitch(Vector3Int current, Vector3Int target) {
    if (Math.Abs(target.x - current.x) == 1 ^ Math.Abs(target.y - current.y) == 1) {
      return true;
    }
    return false;
  }
  // TODO: Animation for dragging tiles (gold glow)?
}
