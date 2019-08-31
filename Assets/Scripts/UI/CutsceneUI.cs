using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneUI : MonoBehaviour
{
    public Sprite[] cutsceneStills;
    int curStillIndex = 0;
    public Image cutsceneStill;
    public TextMeshProUGUI pageCounterText;
    public Image loadingOverlay;

    void Start()
    {
        cutsceneStill.sprite = cutsceneStills[curStillIndex];
        pageCounterText.text = (curStillIndex + 1).ToString() + "/" + cutsceneStills.Length.ToString();
    }

    public void NextStill()
    {
        curStillIndex++;
        if (curStillIndex > cutsceneStills.Length - 1)
        {
            loadingOverlay.gameObject.SetActive(true);
            GameMaster.instance.LoadSceneRaw("LevelSelect");
        }
        else
        {
            cutsceneStill.sprite = cutsceneStills[curStillIndex];
            pageCounterText.text = (curStillIndex + 1).ToString() + "/" + cutsceneStills.Length.ToString();
        }
    }

    public void PreviousStill()
    {
        if (curStillIndex > 0)
        {
            curStillIndex--;
            cutsceneStill.sprite = cutsceneStills[curStillIndex];
            pageCounterText.text = (curStillIndex + 1).ToString() + "/" + cutsceneStills.Length.ToString();
        }
    }
}