using UnityEngine;
using UnityEngine.UI;

public class UIOnSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private GameObject levelTileMap = null;
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
            GameMaster.instance.InventoryItemSelected(prefabToSpawnClone); 
            AttachInventoryItemToMouseLocation(prefabToSpawnClone); 
        }

        if (numOfInventoryItemsController.NumOfItemInInventory <= 0)
        {
            //Debug.Log(gameObject.name + " " + numOfInventoryItemsController.NumOfItemInInventory);
            HandleNoItemsLeft(); 
        }
    }

    public void AttachInventoryItemToMouseLocation(GameObject itemToAttach)
    {
        //Debug.Log(gameObject.name + " is calling me");

        /* if (placingInventoryItem == false)
            placingInventoryItem = true; */

        Cursor.visible = false; 
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0; 
        itemToAttach.transform.position = screenPoint; 

        if (Input.GetMouseButtonDown(0) && mapHandlerExp.GetIfInsideTileGrid((int)screenPoint.x, (int)screenPoint.y))
        {
            PlaceInventoryItemDown(screenPoint, itemToAttach);
            GameMaster.instance.InventoryItemDeselected(); 
            Cursor.visible = true; 
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("backspace"))
        {
            placingInventoryItem = false; 
            Destroy(itemToAttach);
            GameMaster.instance.InventoryItemDeselected(); 
            //GameMaster.Instance.TileMapInventoryItemSelected = false;
            Cursor.visible = true; 
        }

    }

    private void PlaceInventoryItemDown(Vector3 mousePosition, GameObject inventoryItem)
    {   
        Vector3 positionToPlaceItem = mousePosition.RoundXAndYCoords(); 
        inventoryItem.transform.position = positionToPlaceItem; 
        mapHandlerExp.GetTileGrid()[(int)positionToPlaceItem.x, (int)positionToPlaceItem.y] = inventoryItem.GetComponent<Tile>(); 
        GameMaster.instance.AddInventoryItemToMap(gameObject); 
        //Debug.Log("Placing item down");
        //GameMaster.Instance.TileMapInventoryItemSelected = false;
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
    
    public GameObject GetInventoryItemPrefab()
    {
        return prefabToSpawn;
    }

    /* public bool GetIfPlacingInventoryItem()
    { 
        return placingInventoryItem; 
    } */
}
