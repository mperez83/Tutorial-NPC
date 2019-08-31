using UnityEngine;
using UnityEngine.UI;

public class UIOnSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private GameObject levelTileMap = null;
    private GameObject prefabToSpawnClone;
    private bool placingInventoryItem;
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
            GameMaster.instance.placingUIItem = true; 
            AttachInventoryItemToMouseLocation(prefabToSpawnClone);
        }

        if (numOfInventoryItemsController.NumOfItemInInventory <= 0
            && GameMaster.instance.TileMapInventoryItemSelected == false)
        {
            HandleNoItemsLeft();
        }

    }

    public void AttachInventoryItemToMouseLocation(GameObject itemToAttach)
    {
        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        itemToAttach.transform.position = screenPoint;

        int x = Mathf.RoundToInt(screenPoint.x);
        int y = Mathf.RoundToInt(screenPoint.y);

        if (Input.GetMouseButtonDown(0) &&
            mapHandlerExp.GetIfInsideTileGrid(x, y) &&
            mapHandlerExp.GetTileAtCoords(x, y) == null &&
            mapHandlerExp.GetEntityAtCoords(x, y) == null)
        {
            PlaceInventoryItemDown(new Vector2(x, y), itemToAttach);
            GameMaster.instance.InventoryItemDeselected();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("backspace"))
        {
            placingInventoryItem = false;
            Destroy(itemToAttach);
            GameMaster.instance.InventoryItemDeselected();
        }

    }

    private void PlaceInventoryItemDown(Vector3 mousePosition, GameObject inventoryItem)
    {
        Vector3 positionToPlaceItem = mousePosition;
        inventoryItem.transform.position = positionToPlaceItem;
        mapHandlerExp.tileGrid[(int)positionToPlaceItem.x, (int)positionToPlaceItem.y] = inventoryItem.GetComponent<Tile>();
        GameMaster.instance.AddInventoryItemToMap(gameObject);
        GameMaster.instance.placingUIItem = false; 
        placingInventoryItem = false;
    }

    public void OnUIElementSelected()
    {
        if (!placingInventoryItem && button.interactable && 
            GameMaster.instance.placingUIItem == false)
        {
            placingInventoryItem = true;
            prefabToSpawnClone = Instantiate(prefabToSpawn, transform.position,
                Quaternion.identity, levelTileMap.transform);
        }
    }

    private void HandleNoItemsLeft()
    {
        button.interactable = false;
    }

    public GameObject GetInventoryItemPrefab()
    {
        return prefabToSpawn;
    }
}
