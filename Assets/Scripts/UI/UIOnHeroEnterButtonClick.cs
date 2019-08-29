using TMPro;
using UnityEngine;

public class UIOnHeroEnterButtonClick : MonoBehaviour
{
    private MapHandlerExp mapHandlerExp; 
    private TextMeshProUGUI tmproText; 
    private bool heroPhasePaused, mapActivated; 
    private string initialButtonText; 

    private void Awake() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        tmproText = GetComponentInChildren<TextMeshProUGUI>(); 

        initialButtonText = tmproText.text; 
    }

    public void OnHeroEnterButtonClicked()
    {
        if (!mapActivated)
        {
            mapHandlerExp.ActivateMap(); 
            mapActivated = true; 
        }

        if (!heroPhasePaused)
        {
            Time.timeScale = 1f; 
            tmproText.text = "Pause Game";
            heroPhasePaused = true; 
        }    
        else
        {
            Time.timeScale = 0f; 
            tmproText.text = initialButtonText; 
            heroPhasePaused = false; 
        }
    }
}
