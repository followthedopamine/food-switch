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
  [SerializeField] private AudioClip[] musicTracks;
  [SerializeField] private GameObject musicObject;
  private AudioSource musicSource;
  private Vector3 camPos;
  public float sfxVolumePercentage = 1f;
  public float musicVolumePercentage = 1f;
  private float initialMusicVolume;


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
    musicSource = musicObject.GetComponent<AudioSource>();
    initialMusicVolume = musicSource.volume;
    PlayRandomMusicTrack();
    GetPlayerPrefsVolumes();
  }

  private void GetPlayerPrefsVolumes() {
    sfxVolumePercentage = PlayerPrefs.GetFloat("SFXVolume", 1f);
    musicVolumePercentage = PlayerPrefs.GetFloat("MusicVolume", 1f);
    musicSource.volume = initialMusicVolume * musicVolumePercentage;
  }

  public void UpdateMusicVolume(float volume) {
    musicVolumePercentage = volume;
    musicSource.volume = initialMusicVolume * musicVolumePercentage;
    PlayerPrefs.SetFloat("MusicVolume", musicVolumePercentage);
  }

  public void UpdateSFXVolume(float volume) {
    sfxVolumePercentage = volume;
    PlayerPrefs.SetFloat("SFXVolume", sfxVolumePercentage);
  }

  public void PlayRandomMusicTrack() {
    AudioClip trackToPlay = musicTracks[Random.Range(0, musicTracks.Length)];
    if (trackToPlay != musicSource.clip) {
      musicSource.clip = trackToPlay;
      musicSource.Play();
    }
  }

  private void PlayClipAtPointWithVolume(AudioClip sound, float volume) {
    if (sfxVolumePercentage > 0) {
      float soundLog = 10;
      Vector3 camPosAdjusted = new Vector3(camPos.x, camPos.y, camPos.z - (1.0f - volume) * soundLog);
      AudioSource.PlayClipAtPoint(sound, camPosAdjusted);
    }
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
    PlayClipAtPointWithVolume(gameStartSound, sfxVolumePercentage - 0.5f);
  }
}
