using UnityEngine;
using UnityEngine.UI;

public class TheEndUI : MonoBehaviour
{
    public Image loadingOverlay;

    void Start()
    {
        AudioManager.instance.Play("Victory Scene Soundtrack");
    }

    public void ExitButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("Credits");
    }
}