using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Score : MonoBehaviour {

  enum Trophy {
    Bronze,
    Silver,
    Gold,
    None,
  }

  public int currentScore { get; private set; } = 0;
  private Tilemap scoreTilemap;
  private Vector3Int scoreTilePosition;
  [SerializeField] private Sprite[] trophyImages;
  [SerializeField][Tooltip("In order bronze, silver, gold")] private int[] trophyScores;
  private GameObject bottomTrophy;
  private GameObject topTrophy;
  private SpriteRenderer bottomTrophySprite;
  private SpriteRenderer topTrophySprite;
  [SerializeField] private GameObject spriteMask;

  void Start() {
    if (trophyScores.Length != 3) {
      Debug.LogError("Trophy score length should be 3");
    }
    scoreTilemap = GameObject.FindGameObjectWithTag("Score").GetComponent<Tilemap>();
    scoreTilePosition = TileUtil.GetFirstTilePosition(scoreTilemap);
    Tile trophyTile = scoreTilemap.GetTile<Tile>(scoreTilePosition);
    bottomTrophy = TileUtil.ReplaceTileWithGameObject(scoreTilemap, scoreTilePosition);
    bottomTrophySprite = bottomTrophy.GetComponent<SpriteRenderer>();
    scoreTilemap.SetTile(scoreTilePosition, trophyTile);
    topTrophy = TileUtil.ReplaceTileWithGameObject(scoreTilemap, scoreTilePosition);
    topTrophySprite = topTrophy.GetComponent<SpriteRenderer>();
    UpdateTrophyImage();
  }

  Trophy GetNextTrophy() {
    for (int i = 0; i < trophyScores.Length; i++) {
      int threshold = trophyScores[i];
      if (currentScore < threshold) {
        return (Trophy)i;
      }
    }
    return Trophy.None;
  }

  Trophy GetCurrentTrophy() {
    for (int i = 0; i < trophyScores.Length; i++) {
      int threshold = trophyScores[i];
      if (currentScore < threshold) {
        if (i != 0) {
          return (Trophy)i - 1;
        } else {
          break;
        }
      }
    }
    return Trophy.None;
  }

  Sprite GetTrophyImage(Trophy trophy) {
    return trophyImages[(int)trophy];
  }

  int GetTrophyScore(Trophy trophy) {
    return trophyScores[(int)trophy];
  }

  float GetCurrentTrophyProgress() {
    int nextTrophy = GetTrophyScore(GetNextTrophy());
    int currentTrophy = GetTrophyScore(GetCurrentTrophy());
    int pointsRequired = nextTrophy - currentTrophy;
    int pointsSoFar = currentScore - currentTrophy;
    return pointsSoFar / pointsRequired * 100;
  }

  void UpdateTrophyImage() {
    Trophy bottomTrophy = GetNextTrophy();
    Trophy topTrophy = GetCurrentTrophy();
    // tile.sprite = GetTrophyImage(trophy);
    // Bottom tile = next trophy
    bottomTrophySprite.sprite = GetTrophyImage(bottomTrophy);
    topTrophySprite.sprite = GetTrophyImage(topTrophy);
  }

  void AddScore(int scoreToAdd) {
    currentScore += scoreToAdd;
    // UpdateTrophyImage();
  }
}
