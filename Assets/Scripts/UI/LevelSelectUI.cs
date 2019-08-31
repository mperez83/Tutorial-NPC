using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectUI : MonoBehaviour
{
    public Button[] levelButtons;
    public Button secretButton;
    public Image loadingOverlay;

    void Start()
    {
        GameMaster.LevelData[] levelData = GameMaster.instance.levels;

        //Set if the level button is available one by one
        if (levelButtons.Length != levelData.Length) print("Error: there's " + levelData.Length + " levels, and exactly " + levelButtons.Length + " level buttons! fix that");
        for (int i = 0; i < levelData.Length; i++)
            levelButtons[i].interactable = levelData[i].isAvailable;

        //Check if the player has beaten all of the levels
        bool beatenAllLevels = true;
        for (int i = 0; i < levelData.Length; i++)
            if (levelData[i].beaten == false) beatenAllLevels = false;

        if (beatenAllLevels) secretButton.gameObject.SetActive(true);
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

    public void SecretButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("Level 6-6-6");
    }
}