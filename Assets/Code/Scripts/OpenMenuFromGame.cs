using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenuFromGame : MonoBehaviour {
  void OnMouseDown() {
    OpenMenu();
  }

  void OpenMenu() {
    UIController.Instance.ShowInGameMenu();
  }
}
