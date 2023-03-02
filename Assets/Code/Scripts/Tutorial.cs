using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
  private int tutorialIndex = 0;
  [SerializeField] private List<GameObject> tutorialCards;

  public void NextTutorial() {
    tutorialIndex++;
    DisplayCurrentCard();
  }

  public void PreviousTutorial() {
    tutorialIndex--;
    DisplayCurrentCard();
  }

  public void SkipTutorial() {
    // Play level 1
    SceneController.Instance.LoadFirstLevel();
    SceneController.Instance.UnloadTutorial();
  }

  void DisplayCurrentCard() {
    tutorialIndex = tutorialIndex % tutorialCards.Count;
    for (int i = 0; i < tutorialCards.Count; i++) {
      if (i == tutorialIndex) {
        tutorialCards[i].SetActive(true);
      } else {
        tutorialCards[i].SetActive(false);
      }
    }
  }
}
