using UnityEngine;

public class UIOnSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;
    [SerializeField]
    private GameObject levelTileMap;
    private GameObject prefabToSpawnClone; 
    private bool placingInventoryItem; 
    private MapHandlerExp mapHandlerExp; 
    private UIHighlightSelectedTile _UIHighlightSelectedTile; 

    private void Awake() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        _UIHighlightSelectedTile = FindObjectOfType<UIHighlightSelectedTile>(); 
    }

    private void LateUpdate()  
    {
        if (placingInventoryItem)
        {
            _UIHighlightSelectedTile.HighlightSelectedTile(); 
            //GameMaster.Instance.InventoryItemSelected(); 
            AttachInventoryItemToMouseLocation(); 
        }    
    }

    private void AttachInventoryItemToMouseLocation()
    {
        Cursor.visible = false; 
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0; 
        prefabToSpawnClone.transform.position = screenPoint; 

        if (Input.GetMouseButtonDown(0))
        {
            PlaceInventoryItemDown(screenPoint, prefabToSpawnClone);
            _UIHighlightSelectedTile.DeleteHighlightPrefab(); 
            //GameMaster.Instance.InventoryItemDeselected(); 
            Cursor.visible = true; 
        }
    }

    private void PlaceInventoryItemDown(Vector3 mousePosition, GameObject inventoryItem)
    {   
        Vector3 positionToPlaceItem = mousePosition.RoundXAndYCoords(); 
        inventoryItem.transform.position = positionToPlaceItem; 
        mapHandlerExp.mapGrid[(int)positionToPlaceItem.x, (int)positionToPlaceItem.y] = prefabToSpawnClone.GetComponent<Tile>(); 
        placingInventoryItem = false; 
    }

    public void OnUIElementSelected()
    {
        if (!placingInventoryItem)
        {
            placingInventoryItem = true; 
            prefabToSpawnClone = Instantiate(prefabToSpawn, transform.position, 
                Quaternion.identity, levelTileMap.transform);
        }
    }
}
