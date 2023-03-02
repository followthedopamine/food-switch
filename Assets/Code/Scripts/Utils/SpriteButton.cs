using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteButton : MonoBehaviour {
  // Requires (Box) Collider 2D on GameObject to work


  public UnityEvent onClick;

  void OnMouseDown() {
    Debug.Log("Sprite Button pressed");
    onClick.Invoke();
  }
}
