using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Image loadingOverlay;

    public void PlayButton()
    {
        loadingOverlay.enabled = true;

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

    public void QuitButton()
    {
        //Application.Quit();
    }
}