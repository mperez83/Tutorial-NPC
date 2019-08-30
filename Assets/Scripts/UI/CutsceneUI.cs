using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneUI : MonoBehaviour
{
    public Sprite[] cutsceneStills;
    int curStillIndex = 0;
    public Image cutsceneStill;

    void Start()
    {
        cutsceneStill.sprite = cutsceneStills[curStillIndex];
    }

    public void NextStill()
    {
        curStillIndex++;
        if (curStillIndex > cutsceneStills.Length - 1)
        {
            SceneManager.LoadScene("LevelSelect");
        }
        else
        {
            cutsceneStill.sprite = cutsceneStills[curStillIndex];
        }
    }
}