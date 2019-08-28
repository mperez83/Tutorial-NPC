using UnityEngine;

public class UIOnHeroEnterButtonClick : MonoBehaviour
{
    private MapHandlerExp mapHandlerExp; 

    private void Awake() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();     
    }

    public void OnHeroEnterButtonClicked()
    {
        mapHandlerExp.ActivateMap(); 
    }
}
