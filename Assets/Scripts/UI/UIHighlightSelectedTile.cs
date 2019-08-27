using UnityEngine;

public class UIHighlightSelectedTile : MonoBehaviour
{
    [SerializeField]
    private GameObject tileHighlightImage; 
    private GameObject tileHighlightImageClone; 

    private void Start() 
    {
        GameMaster.Instance.OnInventoryItemSelected += HighlightSelectedTile;  
        GameMaster.Instance.OnInventoryItemDeselected += DeleteHighlightPrefab;    
    }

    private void HighlightSelectedTile()
    {
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0; 

        tileHighlightImageClone = Instantiate(tileHighlightImage, screenPoint.RoundXAndYCoords(), 
            Quaternion.identity, transform);
        Debug.Log("I'm getting to the instantiate");
    }

    private void DeleteHighlightPrefab()
    {
        Destroy(tileHighlightImageClone); 
    }

    private void OnDestroy() 
    {
        GameMaster.Instance.OnInventoryItemSelected -= HighlightSelectedTile;
        GameMaster.Instance.OnInventoryItemDeselected -= DeleteHighlightPrefab;     
    }
}
