using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngameCanvasUI : MonoBehaviour
{
    MapHandlerExp mapHandlerExp;
    bool mapActivated;
    bool paused;

    public Image buttonImage;
    public Sprite pauseSprite;
    public Sprite playSprite;
    public TextMeshProUGUI levelName;

    public Image loadingOverlay;



    void Start()
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        levelName.text = GameMaster.instance.GetCurrentSceneName();
    }

    public void PlayButton()
    {
        if (!mapActivated)
        {
            mapHandlerExp.ActivateMap();
            mapActivated = true;
            buttonImage.sprite = pauseSprite;
            AudioManager.instance.Play("Hero Phase Soundtrack");
        }
        else
        {
            //Pause the game
            if (!paused)
            {
                paused = true;
                buttonImage.sprite = playSprite;
                Time.timeScale = 0;
                AudioManager.instance.Play("Building Soundtrack");
            }

            //Unpause the game
            else
            {
                paused = false;
                buttonImage.sprite = pauseSprite;
                Time.timeScale = 1;
                AudioManager.instance.Play("Hero Phase Soundtrack");
            }
        }
    }

    public void RestartButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.ReloadCurrentScene();
        AudioManager.instance.Play("Building Soundtrack");
    }

    public void ExitButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("LevelSelect");
        AudioManager.instance.Play("Menu Scene Soundtrack");
    }
}