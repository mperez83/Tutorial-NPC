using UnityEngine;

public class UIOnTileMapItemSelected : MonoBehaviour
{

    public enum InventoryItems { TurnUp, TurnDown, TurnLeft, TurnRight };

    private InventoryItems inventoryItems;
    private MapHandlerExp mapHandlerExp;
    private UIOnSelection[] _UIInventoryItems;
    private string[] inventoryItemNames;
    private GameObject selectedUIInventoryPrefabGameObject, selectedInventoryItem;
    private bool firstTimeSelected = true;
    public bool placingTileItem = false; 
    private Vector3 mouseScreenPosition;
    //private Tile.TileType[] inventoryItemsList; 

    private void Awake()
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        _UIInventoryItems = GetComponentsInChildren<UIOnSelection>();
        inventoryItemNames = System.Enum.GetNames(typeof(InventoryItems));
        //inventoryItemsList = GetInventoryItems(); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            !GameMaster.instance.placingUIItem &&
            GameMaster.instance.VictoryScreenActivated == false)
        {

            mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseScreenPosition.z = 0;

            mouseScreenPosition = mouseScreenPosition.RoundXAndYCoords();

            if (mapHandlerExp.GetIfInsideTileGrid((int)mouseScreenPosition.x, (int)mouseScreenPosition.y) &&
                mapHandlerExp.GetIfTileExistsInLocation((int)mouseScreenPosition.x, (int)mouseScreenPosition.y))
            {
                if (CheckIfTileInventoryItemSelected(inventoryItemNames,
                    mapHandlerExp.tileGrid[(int)mouseScreenPosition.x, (int)mouseScreenPosition.y]))
                {
                    GameMaster.instance.TileMapInventoryItemSelected = true;
                    selectedInventoryItem = mapHandlerExp.tileGrid
                        [(int)mouseScreenPosition.x, (int)mouseScreenPosition.y].
                        gameObject;

                    Debug.Log("I'm being called");
                    selectedUIInventoryPrefabGameObject =
                        FindInventoryGameObjectAssociatedWithSelectedPrefab(
                            selectedInventoryItem);
                }
            }
        }

        if (GameMaster.instance.TileMapInventoryItemSelected)
        {
            GameMaster.instance.placingUIItem = true;
            //placingTileItem = true;
            GameMaster.instance.InventoryItemSelected(selectedUIInventoryPrefabGameObject);
            if (!firstTimeSelected)
                selectedUIInventoryPrefabGameObject.GetComponent<UIOnSelection>().
                    AttachInventoryItemToMouseLocation(selectedInventoryItem);
            else
            {
                RemoveItemFromTileGrid();
                firstTimeSelected = false;
            }
        }
        else
        {
            GameMaster.instance.placingUIItem = false;
            //placingTileItem = false;
            firstTimeSelected = true;
        }
    }

    private void RemoveItemFromTileGrid()
    {
        mapHandlerExp.tileGrid
            [(int)mouseScreenPosition.x, (int)mouseScreenPosition.y] = null;
    }

    private GameObject FindInventoryGameObjectAssociatedWithSelectedPrefab(GameObject selectedPrefab)
    {
        GameObject inventoryGameObejctAssociatedWithSelectedPrefab = null;
        foreach (UIOnSelection UIInventoryItem in _UIInventoryItems)
        {
            if (UIInventoryItem.GetInventoryItemPrefab().GetComponent<Tile>().tileType == selectedPrefab.GetComponent<Tile>().tileType)
                inventoryGameObejctAssociatedWithSelectedPrefab = UIInventoryItem.gameObject;
        }
        return inventoryGameObejctAssociatedWithSelectedPrefab;
    }

    // if Tile.TyleType = a Tile.TyleType in inventoryItems, then return true
    private bool CheckIfTileInventoryItemSelected(string[] inventoryItems, Tile selectedTile)
    {
        bool selectedItemIsAnInventoryItem = false;

        foreach (var inventoryItem in inventoryItems)
        {
            if (selectedTile.tileType.ToString() == inventoryItem)
            {
                selectedItemIsAnInventoryItem = true;
            }
        }

        return selectedItemIsAnInventoryItem;
    }
}