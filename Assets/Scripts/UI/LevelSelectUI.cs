using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectUI : MonoBehaviour
{
    public Button[] levelButtons;
    public Image loadingOverlay;

    void Start()
    {
        GameMaster.LevelData[] levelData = GameMaster.instance.levels;

        //Set if the level button is available one by one
        if (levelButtons.Length != levelData.Length) print("Error: there's " + levelData.Length + " levels, and exactly " + levelButtons.Length + " level buttons! fix that");
        for (int i = 0; i < levelData.Length; i++)
            levelButtons[i].interactable = levelData[i].isAvailable;
    }

    public void LevelButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        AudioManager.instance.Play("Building Soundtrack");
        GameMaster.instance.SetLevel(EventSystem.current.currentSelectedGameObject.name);
    }

    public void MainMenuButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("MainMenu");
    }
}