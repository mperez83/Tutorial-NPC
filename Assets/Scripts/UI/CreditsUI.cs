using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    public Image loadingOverlay;

    public void ExitButton()
    {
        loadingOverlay.gameObject.SetActive(true);
        GameMaster.instance.LoadSceneRaw("MainMenu");
    }
}