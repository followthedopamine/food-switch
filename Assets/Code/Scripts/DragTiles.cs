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

  void Start() {
    levelTilemap = gameObject.GetComponent<Tilemap>();
    cam = Camera.main;
    levelController = gameObject.GetComponent<LevelController>();
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

  private Vector3Int GetTileInDraggedDirection(Vector3 mousePos) {
    Vector3Int draggingTo = levelTilemap.WorldToCell(mousePos);
    if (draggingTo == draggingFrom) return draggingTo;
    Vector3Int draggingDirection = draggingTo - draggingFrom;
    Vector3Int absDraggingDirection = new Vector3Int(Math.Abs(draggingDirection.x), Math.Abs(draggingDirection.y), 0);
    if (absDraggingDirection.x > absDraggingDirection.y) {
      if (draggingFrom.x > draggingTo.x) {
        return new Vector3Int(draggingFrom.x - 1, draggingFrom.y, 0);
      } else {
        return new Vector3Int(draggingFrom.x + 1, draggingFrom.y, 0);
      }
    } else {
      if (draggingFrom.y > draggingTo.y) {
        return new Vector3Int(draggingFrom.x, draggingFrom.y - 1, 0);
      } else {
        return new Vector3Int(draggingFrom.x, draggingFrom.y + 1, 0);
      }
    }
  }

  private void SwitchWithSelectedTile() {
    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    // Vector3Int draggingTo = levelTilemap.WorldToCell(mousePos);
    Vector3Int draggingTo = GetTileInDraggedDirection(mousePos);
    if (draggingTo == draggingFrom) return;
    targetTile = levelTilemap.GetTile<GameTile>(draggingTo);
    if (ValidateSwitch(draggingTo, draggingFrom) && targetTile != null) {
      levelTilemap.SetTile(draggingTo, draggedTile);
      levelTilemap.SetTile(draggingFrom, targetTile);
    }
    levelController.OnSwitch();
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
