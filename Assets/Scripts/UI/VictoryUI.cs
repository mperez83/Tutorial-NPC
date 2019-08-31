using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public Image loadingOverlay;

    public void ActivateVictoryScreen()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        gameOverText.text = SceneManager.GetActiveScene().name + " completed!";
        GameMaster.instance.SetNextLevelAvailable();
    }

    public void RetryButton()
    {
        Time.timeScale = 1;
        loadingOverlay.enabled = true;
        AudioManager.instance.Play("Building Soundtrack");
        GameMaster.instance.LoadSceneRaw(SceneManager.GetActiveScene().name);
    }

    public void NextLevelButton()
    {
        Time.timeScale = 1;
        loadingOverlay.enabled = true;
        AudioManager.instance.Play("Building Soundtrack");
        GameMaster.instance.AdvanceLevel();
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        loadingOverlay.enabled = true;
        AudioManager.instance.Play("Menu Scene Soundtrack");
        GameMaster.instance.LoadSceneRaw("LevelSelect");
    }
}