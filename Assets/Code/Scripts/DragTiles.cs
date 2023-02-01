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
  private GameTile draggedTile;
  private Tile targetTile;
  private LevelController levelController;
  [SerializeField] private GameObject tileSwitchIndicatorPrefab;
  [SerializeField] private Sprite cancelSprite;
  private Sprite switchSprite;
  private GameObject tileSwitchIndicator;
  private bool shouldDisplayTileSwitchIndicator = false;
  private SpriteRenderer tileSwitchIndicatorSprite;
  private float tileSwitchHeight;
  private float tileSwitchWidth;
  private Vector3Int topRight;
  private Vector3Int bottomLeft;

  void Start() {
    GameObject levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelTilemap = gameObject.GetComponent<Tilemap>();
    cam = Camera.main;
    levelController = gameObject.GetComponent<LevelController>();
    tileSwitchIndicator = Instantiate(tileSwitchIndicatorPrefab);
    tileSwitchIndicator.SetActive(false);
    tileSwitchIndicatorSprite = tileSwitchIndicator.GetComponent<SpriteRenderer>();
    switchSprite = tileSwitchIndicatorSprite.sprite;
    tileSwitchWidth = tileSwitchIndicatorSprite.bounds.size.x;
    tileSwitchHeight = tileSwitchIndicatorSprite.bounds.size.y;
    topRight = levelController.level.grid[levelController.level.height - 1, levelController.level.width - 1];
    bottomLeft = levelController.level.grid[0, 0];
  }

  private void Update() {
    if (shouldDisplayTileSwitchIndicator)
      DisplayTileSwitchIndicator();
  }

  private void OnMouseDown() {
    // Should help with touch input later
    if (levelController.isLoopRunning) return;
    if (UIController.Instance.isPaused) return;
    SelectTileForSwitch();
    shouldDisplayTileSwitchIndicator = true;
    // Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    // Debug.Log(levelTilemap.WorldToCell(mousePos));
  }

  private void OnMouseUp() {
    if (levelController.isLoopRunning) return;
    if (UIController.Instance.isPaused) return;
    SwitchWithSelectedTile();
    HideTileSwitchIndicator();
  }

  void HideTileSwitchIndicator() {
    tileSwitchIndicator.SetActive(false);
    shouldDisplayTileSwitchIndicator = false;
  }
  void DisplayTileSwitchIndicator() {
    tileSwitchIndicatorSprite.sprite = switchSprite;
    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    Vector3Int draggingTo = GetTileInDraggedDirection(mousePos);
    GameTile draggingToTile = levelTilemap.GetTile<GameTile>(draggingTo);
    Vector3Int direction = new Vector3Int(draggingTo.x - draggingFrom.x, draggingTo.y - draggingFrom.y, draggingFrom.z);

    if (!CanBeDragged(draggedTile) || !CanBeDragged(draggingToTile)) {
      tileSwitchIndicatorSprite.sprite = cancelSprite;
      tileSwitchIndicator.SetActive(true);
      tileSwitchIndicator.transform.position = levelTilemap.CellToWorld(new Vector3Int(
        draggingFrom.x,
        draggingFrom.y + 1,
        draggingFrom.z)
      );
      return;
    }

    // This part is baaaaaaaad
    if (direction.x == 1) {
      if (draggingFrom.x == topRight.x) {
        tileSwitchIndicatorSprite.sprite = cancelSprite;
        tileSwitchIndicator.SetActive(true);
        return;
      }
      tileSwitchIndicator.transform.localRotation = Quaternion.Euler(0, 0, 0);
      tileSwitchIndicator.transform.position = levelTilemap.CellToWorld(new Vector3Int(
        draggingFrom.x,
        draggingFrom.y + 1,
        draggingFrom.z)
      );
    }
    if (direction.x == -1) {
      if (draggingFrom.x == bottomLeft.x) {
        tileSwitchIndicatorSprite.sprite = cancelSprite;
        tileSwitchIndicator.SetActive(true);
        return;
      }
      tileSwitchIndicator.transform.localRotation = Quaternion.Euler(0, 0, 180);
      tileSwitchIndicator.transform.position = levelTilemap.CellToWorld(new Vector3Int(
        draggingFrom.x + 1,
        draggingFrom.y,
        draggingFrom.z)
      );
    }
    if (direction.y == -1) {
      if (draggingFrom.y == bottomLeft.y) {
        tileSwitchIndicatorSprite.sprite = cancelSprite;
        tileSwitchIndicator.SetActive(true);
        return;
      }
      tileSwitchIndicator.transform.localRotation = Quaternion.Euler(0, 0, 270);
      tileSwitchIndicator.transform.position = levelTilemap.CellToWorld(new Vector3Int(
        draggingFrom.x + 1,
        draggingFrom.y + 1,
        draggingFrom.z)
      );
    }
    if (direction.y == 1) {
      if (draggingFrom.y == topRight.y) {
        tileSwitchIndicatorSprite.sprite = cancelSprite;
        tileSwitchIndicator.SetActive(true);
        return;
      }
      tileSwitchIndicator.transform.localRotation = Quaternion.Euler(0, 0, 90);
      tileSwitchIndicator.transform.position = levelTilemap.CellToWorld(new Vector3Int(
        draggingFrom.x,
        draggingFrom.y,
        draggingFrom.z)
      );
    }

    if (direction.x == 0 && direction.y == 0) {
      tileSwitchIndicator.transform.localRotation = Quaternion.Euler(0, 0, 0);
      tileSwitchIndicator.transform.position = levelTilemap.CellToWorld(new Vector3Int(
        draggingFrom.x,
        draggingFrom.y + 1,
        draggingFrom.z)
      );
      tileSwitchIndicatorSprite.sprite = cancelSprite;
    }
    tileSwitchIndicator.SetActive(true);
  }

  private void SelectTileForSwitch() {
    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    draggingFrom = levelTilemap.WorldToCell(mousePos);
    draggedTile = levelTilemap.GetTile<GameTile>(draggingFrom);
  }

  private bool CanBeDragged(GameTile tile) {
    if (tile.type == GameTile.Type.Breakable) {
      return false;
    }
    return true;
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
      levelController.OnSwitch(draggingFrom, draggingTo);
      SoundController.Instance.PlaySwitchSound();
    }
  }

  // This might need to be in a different class
  private bool ValidateSwitch(Vector3Int current, Vector3Int target) {
    if (Math.Abs(target.x - current.x) == 1 ^ Math.Abs(target.y - current.y) == 1) {
      GameTile currentTile = levelTilemap.GetTile<GameTile>(current);
      GameTile targetTile = levelTilemap.GetTile<GameTile>(target);
      if (CanBeDragged(currentTile) && CanBeDragged(targetTile)) return true;
    }
    return false;
  }
}

