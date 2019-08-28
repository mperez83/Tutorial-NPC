using UnityEngine;
using UnityEngine.UI;

public class UIOnSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;
    [SerializeField]
    private GameObject levelTileMap;
    private GameObject prefabToSpawnClone; 
    private bool placingInventoryItem, noInventoryItemsLeft; 
    private MapHandlerExp mapHandlerExp;
    private NumberOfInventoryItemsController numOfInventoryItemsController; 
    private Button button; 

    private void Awake() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        numOfInventoryItemsController = GetComponent<NumberOfInventoryItemsController>(); 
        button = GetComponent<Button>(); 
    }

    private void LateUpdate()  
    {
        if (placingInventoryItem)
        {
            GameMaster.Instance.InventoryItemSelected(); 
            AttachInventoryItemToMouseLocation(); 
        }

        if (numOfInventoryItemsController.NumOfItemInInventory <= 0)
        {
            Debug.Log(gameObject.name + " " + numOfInventoryItemsController.NumOfItemInInventory);
            HandleNoItemsLeft(); 
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
            GameMaster.Instance.InventoryItemDeselected(); 
            Cursor.visible = true; 
        }
    }

    private void PlaceInventoryItemDown(Vector3 mousePosition, GameObject inventoryItem)
    {   
        Vector3 positionToPlaceItem = mousePosition.RoundXAndYCoords(); 
        inventoryItem.transform.position = positionToPlaceItem; 
        mapHandlerExp.GetTileGrid()[(int)positionToPlaceItem.x, (int)positionToPlaceItem.y] = prefabToSpawnClone.GetComponent<Tile>(); 
        GameMaster.Instance.AddInventoryItemToMap(gameObject); 
        placingInventoryItem = false; 
    }

    public void OnUIElementSelected()
    {
        if (!placingInventoryItem && !noInventoryItemsLeft)
        {
            placingInventoryItem = true; 
            prefabToSpawnClone = Instantiate(prefabToSpawn, transform.position, 
                Quaternion.identity, levelTileMap.transform);
        }
    }

    private void HandleNoItemsLeft()
    {
        noInventoryItemsLeft = true; 
        button.interactable = false; 
    }
}
