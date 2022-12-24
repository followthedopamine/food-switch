using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalText : MonoBehaviour {
  private TMP_Text goalText;

  void Start() {
    goalText = gameObject.GetComponent<TMP_Text>();
  }

  public void UpdateText(int goalCompletion, int goalTarget) {
    goalText.text = goalCompletion + "/" + goalTarget;
  }
}
