using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    public void ActivateGameOver(string causeOfGameOver)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        gameOverText.text = causeOfGameOver;
    }

    public void RetryButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}