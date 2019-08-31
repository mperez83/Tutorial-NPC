using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Image loadingOverlay;
    public Button viewCutsceneButton;

    void Start()
    {
        if (GameMaster.instance.seenCutscene)
        {
            viewCutsceneButton.gameObject.SetActive(true);
        }
    }

    public void PlayButton()
    {
        loadingOverlay.gameObject.SetActive(true);

        if (!GameMaster.instance.seenCutscene)
        {
            GameMaster.instance.seenCutscene = true;
            GameMaster.instance.LoadSceneRaw("Cutscene");
        }
        else
        {
            GameMaster.instance.LoadSceneRaw("LevelSelect");
        }
    }

    public void CreditsButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("Credits");
    }

    public void ViewIntroButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("Cutscene");
    }
}