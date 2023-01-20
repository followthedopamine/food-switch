using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LineClear : MonoBehaviour {
  private Vector3Int topRight;
  private Vector3Int bottomLeft;
  private LevelController levelController;
  private DestroyMatches destroyMatches;
  private Tilemap levelTilemap;
  private int powerupOffset = 900;


  private void Start() {
    GameObject levelObject = GameObject.FindGameObjectWithTag("LevelController");
    levelTilemap = levelObject.GetComponent<Tilemap>();
    levelController = levelObject.GetComponent<LevelController>();
    destroyMatches = gameObject.GetComponent<DestroyMatches>();
    // topRight = levelController.level.grid[levelController.level.height - 1, levelController.level.width - 1];
    // bottomLeft = levelController.level.grid[0, 0];

  }

  // Check if tile is switched vertically or horizontally
  // Mark all tiles as a match in vertical or horizontal line from finish point of tile
  // Display some kind of animation

  private bool WasTileSwitchedVertically(Vector3Int position, Vector3Int oldPosition) {
    if (position.y == oldPosition.y) return false;
    return true;
  }

  private Match CreateLineMatch(Vector3Int position, bool vertical) {
    Match line = new Match();
    line.tileId = -1;
    line.tiles = new List<Vector3Int>();
    line.location = position;
    if (vertical) {
      for (int i = 0; i < levelController.level.height - 1; i++) {
        GameTile tile = levelTilemap.GetTile<GameTile>(levelController.level.grid[i, position.x - levelController.tilemapOffset.x]);
        if (tile == null) continue;
        if (tile.id < powerupOffset) {
          line.size++;
          line.tiles.Add(levelController.level.grid[i, position.x - levelController.tilemapOffset.x]);
        }
      }
    } else {
      for (int i = 0; i < levelController.level.width; i++) {
        GameTile tile = levelTilemap.GetTile<GameTile>(levelController.level.grid[position.y - levelController.tilemapOffset.y, i]);
        if (tile == null) continue;
        if (tile.id < powerupOffset) {
          line.size++;
          line.tiles.Add(levelController.level.grid[position.y - levelController.tilemapOffset.y, i]);
        }
      }
    }
    return line;
  }

  public IEnumerator ClearLine(Vector3Int position, Vector3Int oldPosition) {
    List<Match> lineMatchList = new List<Match>();
    Match line = CreateLineMatch(position, WasTileSwitchedVertically(position, oldPosition));
    lineMatchList.Add(line);
    destroyMatches.DestroyTiles(lineMatchList);
    levelTilemap.SetTile(position, null);
    yield return new WaitForSeconds(0.3f);
  }
}

// public struct Match {
//   public int tileId;
//   public int size;
//   public Vector3Int location;
//   public List<Vector3Int> tiles;
// }