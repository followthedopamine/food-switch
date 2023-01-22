using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectBuilder : MonoBehaviour {

  [SerializeField] private GameObject levelButton;
  private int buttonPadding = 40;
  private int topButtonPosition = 80;
  private UIButtons uiButtons;

  private void Start() {
    CreateLevelSelectMenuItems();
    GameObject UI = GameObject.FindGameObjectWithTag("UI");
    uiButtons = UI.GetComponent<UIButtons>();
  }

  public void CreateLevelSelectMenuItems() {
    for (int i = 0; i < SceneController.Instance.levelList.Count; i++) {
      // Instantiate object, move it down index * 40, change text, change button function
      // Maybe change height of scroll container
      string level = SceneController.Instance.levelList[i];
      CreateLevelButton(i);
    }
  }

  public void CreateLevelButton(int index) {
    GameObject newButton = Instantiate(levelButton);
    newButton.transform.SetParent(levelButton.transform.parent);
    newButton.transform.localPosition = new Vector3(0, topButtonPosition - (buttonPadding * index), 0);
    TMP_Text buttonText = newButton.GetComponent<TMP_Text>();
    buttonText.text = "Level " + (index + 1);
    Button button = newButton.GetComponent<Button>();
    button.onClick.AddListener(() => uiButtons.LevelButton(index));
  }

}
