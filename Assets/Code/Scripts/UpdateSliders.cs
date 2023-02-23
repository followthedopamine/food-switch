using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSliders : MonoBehaviour {

  void OnEnable() {
    UIButtons uiButtons = GameObject.FindGameObjectWithTag("UI").GetComponent<UIButtons>();
    uiButtons.UpdateSliders();
  }
}
