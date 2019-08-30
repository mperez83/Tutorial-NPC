using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadSceneAsync("Cutscene");
    }

    public void QuitButton()
    {
        //Application.Quit();
    }
}