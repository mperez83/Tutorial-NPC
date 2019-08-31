using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectUI : MonoBehaviour
{
    public Transform levelButtonHolder;
    public Image loadingOverlay;

    void Start()
    {
        GameMaster.LevelData[] levelData = GameMaster.instance.levels;

        //Set if the level button is available one by one
        for (int i = 0; i < levelData.Length; i++)
        {
            Transform levelButton = levelButtonHolder.Find(levelData[i].levelName);

            if (!levelButton) print("Error: level button for " + levelData[i].levelName + " is not present!!! go put it in");
            else levelButton.GetComponent<Button>().interactable = levelData[i].isAvailable;
        }
    }

    public void LevelButton()
    {
        loadingOverlay.enabled = true;
        AudioManager.instance.Play("Building Soundtrack");
        GameMaster.instance.SetLevel(EventSystem.current.currentSelectedGameObject.name);
    }

    public void MainMenuButton()
    {
        loadingOverlay.enabled = true;
        AudioManager.instance.Play("Menu Scene Soundtrack");
        GameMaster.instance.LoadSceneRaw("MainMenu");
    }
}