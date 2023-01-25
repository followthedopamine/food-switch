using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
  public static SoundController Instance;
  [SerializeField] private AudioClip switchSound;
  [SerializeField] private AudioClip matchSound;
  [SerializeField] private AudioClip trophyUpSound;
  [SerializeField] private AudioClip gameStartSound;
  [SerializeField] private AudioClip gameWonSound;
  [SerializeField] private AudioClip gameOverSound;
  private Vector3 camPos;
  public float sfxVolumePercentage = 1f;

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
    camPos = Camera.main.transform.position;
  }

  private void PlayClipAtPointWithVolume(AudioClip sound, float volume) {
    float soundLog = 10;
    Vector3 camPosAdjusted = new Vector3(camPos.x, camPos.y, camPos.z - (1.0f - volume) * soundLog);
    AudioSource.PlayClipAtPoint(sound, camPosAdjusted);
  }

  public void PlaySwitchSound() {
    PlayClipAtPointWithVolume(switchSound, sfxVolumePercentage);
  }

  public void PlayMatchSound() {
    PlayClipAtPointWithVolume(matchSound, sfxVolumePercentage - 0.3f);
  }

  public void PlayTrophyUpSound() {
    PlayClipAtPointWithVolume(trophyUpSound, sfxVolumePercentage - 0.4f);
  }

  public void PlayGameWonSound() {
    PlayClipAtPointWithVolume(gameWonSound, sfxVolumePercentage - 0.4f);
  }

  public void PlayGameOverSound() {
    PlayClipAtPointWithVolume(gameOverSound, sfxVolumePercentage);
  }

  public void PlayGameStartSound() {
    PlayClipAtPointWithVolume(gameStartSound, sfxVolumePercentage);
  }
}
