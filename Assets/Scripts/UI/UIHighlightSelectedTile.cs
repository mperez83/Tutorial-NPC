using UnityEngine;

public class UIHighlightSelectedTile : MonoBehaviour
{
    [SerializeField]
    private GameObject tileHighlightImage = null; 
    private GameObject tileHighlightImageClone; 
    private bool tileHighlightInstantiated;

    private MapHandlerExp mapHandlerExp;

    private void Start() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        GameMaster.instance.OnInventoryItemSelected += HighlightSelectedTile;  
        GameMaster.instance.OnInventoryItemDeselected += DeleteHighlightPrefab;
    }

    public void HighlightSelectedTile(GameObject placeholderParameter)
    { 
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0; 

        screenPoint = screenPoint.RoundXAndYCoords(); 

        if (!tileHighlightInstantiated)
        {
            tileHighlightImageClone = Instantiate(tileHighlightImage, screenPoint, 
                Quaternion.identity, transform);
            tileHighlightInstantiated = true; 
        }

        if (mapHandlerExp.GetIfInsideTileGrid((int)screenPoint.x, (int)screenPoint.y))
        {
            if (mapHandlerExp.tileGrid[(int)screenPoint.x, (int)screenPoint.y] == null)
                tileHighlightImageClone.transform.position = screenPoint.RoundXAndYCoords();
        }
    }

    public void DeleteHighlightPrefab()
    {
        tileHighlightInstantiated = false; 
        Destroy(tileHighlightImageClone); 
    }

    private void OnDestroy() 
    {
        GameMaster.instance.OnInventoryItemSelected -= HighlightSelectedTile;
        GameMaster.instance.OnInventoryItemDeselected -= DeleteHighlightPrefab;
    }
}
