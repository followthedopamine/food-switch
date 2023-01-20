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

  public int currentScore { get; private set; } = 150;
  private Tilemap scoreTilemap;
  private Vector3Int scoreTilePosition;
  [SerializeField] private Sprite[] trophyImages;
  [SerializeField][Tooltip("In order bronze, silver, gold")] private int[] trophyScores;
  private GameObject bottomTrophy;
  private GameObject topTrophy;
  private SpriteRenderer bottomTrophySprite;
  private SpriteRenderer topTrophySprite;
  [SerializeField] private GameObject spriteMask;
  [SerializeField] private GameObject levelUpParticlesPrefab;
  private Trophy currentTrophy;
  private float trophySpriteHeight;
  private float trophySpriteWidth;
  private float maskX;
  private float maskY;

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
    topTrophySprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    topTrophySprite.sortingOrder = 10;
    spriteMask = Instantiate(spriteMask);
    spriteMask.transform.position = topTrophy.transform.position;
    spriteMask.transform.localScale = topTrophy.transform.localScale;
    currentTrophy = Trophy.None;
    trophySpriteWidth = topTrophySprite.bounds.size.x;
    trophySpriteHeight = topTrophySprite.bounds.size.y;
    maskX = spriteMask.transform.position.x;
    maskY = spriteMask.transform.position.y;
    UpdateTrophyImage();
  }

  Trophy GetNextTrophy() {
    for (int i = 0; i < trophyScores.Length; i++) {
      int threshold = trophyScores[i];
      if (currentScore < threshold) {
        return (Trophy)i;
      }
    }
    return Trophy.Gold;
  }

  Trophy GetCurrentTrophy() {
    for (int i = 0; i < trophyScores.Length; i++) {
      int threshold = trophyScores[i];
      if (currentScore < threshold) {
        if (i != 0) {
          return (Trophy)i - 1;
        } else {
          return Trophy.None;
        }
      }
    }
    return Trophy.Gold;
  }

  Sprite GetTrophyImage(Trophy trophy) {
    return trophyImages[(int)trophy];
  }

  int GetTrophyScore(Trophy trophy) {
    if (trophy == Trophy.None) return 0;
    return trophyScores[(int)trophy];
  }

  float GetCurrentTrophyProgress() {
    float nextTrophy = GetTrophyScore(GetNextTrophy());
    float currentTrophy = GetTrophyScore(GetCurrentTrophy());
    float pointsRequired = nextTrophy - currentTrophy;
    float pointsSoFar = currentScore - currentTrophy;
    return pointsSoFar / pointsRequired;
  }

  void SpawnTrophyLevelUpParticles() {
    Vector3 positionToSpawn = topTrophy.transform.position;
    GameObject particles = Instantiate(levelUpParticlesPrefab);
    particles.transform.position = positionToSpawn;
  }

  void UpdateTrophyImage() {
    Trophy bottomTrophy = GetCurrentTrophy();
    Trophy topTrophy = GetNextTrophy();
    bottomTrophySprite.sprite = GetTrophyImage(bottomTrophy);
    topTrophySprite.sprite = GetTrophyImage(topTrophy);
    if (bottomTrophy != currentTrophy) {
      currentTrophy = bottomTrophy;
      SpawnTrophyLevelUpParticles();
    }
    if (bottomTrophy != Trophy.Gold) {
      spriteMask.transform.position = new Vector3(
        spriteMask.transform.position.x,
        maskY + GetCurrentTrophyProgress() * trophySpriteHeight
      );
    }
  }

  public void AddScore(int scoreToAdd) {
    currentScore += scoreToAdd;
    UpdateTrophyImage();
  }
}
