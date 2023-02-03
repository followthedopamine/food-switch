using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectBuilder : MonoBehaviour {

  [SerializeField] private GameObject levelButton;
  [SerializeField] private Sprite[] trophyImages;
  private int buttonPadding = 40;
  private int topButtonPosition = 300;
  private UIButtons uiButtons;
  private List<GameObject> buttonList = new List<GameObject>();

  private void Start() {
    GameObject UI = GameObject.FindGameObjectWithTag("UI");
    uiButtons = UI.GetComponent<UIButtons>();
    CreateLevelSelectMenuItems();
  }

  void OnEnable() {
    if (buttonList.Count == 0) return;
    UpdateTrophyImages();
  }

  void UpdateTrophyImages() {
    for (int i = 0; i < buttonList.Count; i++) {
      GameObject child = buttonList[i].transform.Find("Trophy Image").gameObject;
      child.GetComponent<Image>().sprite = trophyImages[GameController.Instance.levelCompletion[i]];
    }
  }

  public void CreateLevelSelectMenuItems() {
    Debug.Log(SceneController.Instance.levelList.Count);
    for (int i = 0; i < SceneController.Instance.levelList.Count; i++) {
      // Instantiate object, move it down index * 40, change text, change button function
      // Maybe change height of scroll container
      string level = SceneController.Instance.levelList[i];
      CreateLevelButton(i);
    }
  }

  private void CreateLevelButton(int index) {
    GameObject newButton = Instantiate(levelButton);
    newButton.transform.SetParent(levelButton.transform.parent);
    newButton.transform.localPosition = new Vector3(0, topButtonPosition - (buttonPadding * index), 0);
    TMP_Text buttonText = newButton.GetComponent<TMP_Text>();
    buttonText.text = "Level " + (index + 1);
    Button button = newButton.GetComponent<Button>();
    button.onClick.AddListener(() => uiButtons.LevelButton(index));
    GameObject child = newButton.transform.Find("Trophy Image").gameObject;
    child.GetComponent<Image>().sprite = trophyImages[GameController.Instance.levelCompletion[index]];
    buttonList.Add(newButton);
  }

}
