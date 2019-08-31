using UnityEngine;
using UnityEngine.UI;

public class UIOnSelection : MonoBehaviour
{

    //public enum InventoryObjectType {UIPanelInventoryItem, TileMapInventoryItem};

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

    private void Start() 
    {
        GameMaster.Instance.OnInventoryItemSelected += AttachInventoryItemToMouseLocation;     
    }

    private void LateUpdate()  
    {
        if (placingInventoryItem)
        {
            GameMaster.Instance.InventoryItemSelected(gameObject, prefabToSpawnClone); 
            //AttachInventoryItemToMouseLocation(); 
        }
        
        if (numOfInventoryItemsController.NumOfItemInInventory <= 0)
        {
            //Debug.Log(gameObject.name + " " + numOfInventoryItemsController.NumOfItemInInventory);
            HandleNoItemsLeft(); 
        }
    }

    private void OnDestroy() 
    {
        GameMaster.Instance.OnInventoryItemSelected -= AttachInventoryItemToMouseLocation;     
    }

    private void AttachInventoryItemToMouseLocation(GameObject inventoryGameObject, GameObject prefabToMove)
    {
        bool proceedAttachingItem = false; 

        Debug.Log(prefabToSpawnClone.name);
        Debug.Log("IGM " + inventoryGameObject.name);

        if (inventoryGameObject == this)
            proceedAttachingItem = true; 
        if (inventoryGameObject.GetComponent<Tile>().tileType != null &&
            prefabToSpawnClone.GetComponent<Tile>().tileType == inventoryGameObject.GetComponent<Tile>().tileType)
        {
            proceedAttachingItem = true; 
        }

        if (proceedAttachingItem)
        {
            if (inventoryGameObject != this)
                prefabToSpawnClone = inventoryGameObject;
            Cursor.visible = false; 
            Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            screenPoint.z = 0; 
            prefabToSpawnClone.transform.position = screenPoint; 

            if (Input.GetMouseButtonDown(0) && 
                mapHandlerExp.GetIfInsideTileGrid((int)screenPoint.x, (int)screenPoint.y))
            {
                PlaceInventoryItemDown(screenPoint, prefabToSpawnClone);
                GameMaster.Instance.InventoryItemDeselected(); 
                Cursor.visible = true; 
            }
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("backspace"))
            {
                placingInventoryItem = false; 
                Destroy(prefabToSpawnClone);

                GameMaster.Instance.InventoryItemDeselected(); 
                GameMaster.Instance.TileMapInventoryItemSelected = false; 

                Cursor.visible = true; 
            }
        }

    }

    private void PlaceInventoryItemDown(Vector3 mousePosition, GameObject inventoryItem)
    {   
        Vector3 positionToPlaceItem = mousePosition.RoundXAndYCoords(); 
        inventoryItem.transform.position = positionToPlaceItem; 
        mapHandlerExp.tileGrid[(int)positionToPlaceItem.x, (int)positionToPlaceItem.y] = prefabToSpawnClone.GetComponent<Tile>(); 
        GameMaster.Instance.AddInventoryItemToMap(gameObject); 
        GameMaster.Instance.TileMapInventoryItemSelected = false;
        placingInventoryItem = false; 
    }

    public void OnUIElementSelected()
    {
        if (!placingInventoryItem && !noInventoryItemsLeft)
        {
            prefabToSpawnClone = Instantiate(prefabToSpawn, transform.position, 
                Quaternion.identity, levelTileMap.transform);
            placingInventoryItem = true; 
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
}