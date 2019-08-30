using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnTileMapItemSelected : MonoBehaviour
{

    public enum InventoryItems { TurnUp, TurnDown, TurnLeft, TurnRight};

    private InventoryItems inventoryItems; 
    private MapHandlerExp mapHandlerExp; 
    private UIOnSelection[] _UIInventoryItems; 
    private string[] inventoryItemNames; 
    private GameObject selectedInventoryPrefabUIGameObject; 
    //private Tile.TileType[] inventoryItemsList; 

    private void Awake() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>(); 
        _UIInventoryItems = GetComponentsInChildren<UIOnSelection>(); 
        inventoryItemNames = System.Enum.GetNames (typeof(InventoryItems));
        //inventoryItemsList = GetInventoryItems(); 
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseScreenPosition.z = 0; 

            mouseScreenPosition = mouseScreenPosition.RoundXAndYCoords(); 

            if (mapHandlerExp.GetIfInsideTileGrid((int)mouseScreenPosition.x, (int)mouseScreenPosition.y) &&
                mapHandlerExp.GetIfTileExistsInLocation((int)mouseScreenPosition.x, (int)mouseScreenPosition.y))
                {
                    if (CheckIfTileInventoryItemSelected(inventoryItemNames, 
                        mapHandlerExp.GetTileGrid()[(int)mouseScreenPosition.x,(int)mouseScreenPosition.y]))
                        {
                            GameMaster.Instance.TileMapInventoryItemSelected = true;
                            selectedInventoryPrefabUIGameObject =
                                FindInventoryGameObjectAssociatedWithSelectedPrefab(
                                    mapHandlerExp.GetTileGrid()[(int)mouseScreenPosition.x,
                                    (int)mouseScreenPosition.y].gameObject);
                            Debug.Log(mapHandlerExp.GetTileGrid()[(int)mouseScreenPosition.x,(int)mouseScreenPosition.y].gameObject.name);
                            //GameMaster.Instance.InventoryItemSelected(gameObject); 
                            Debug.Log("Inventory Item Selected");
                        }
                }
        }

        if (GameMaster.Instance.TileMapInventoryItemSelected)
            GameMaster.Instance.InventoryItemSelected(selectedInventoryPrefabUIGameObject); 
    }

    // 
    /* private Tile.TileType[] GetInventoryItems()
    {
              
        Tile.TileType[] inventoryItems = new Tile.TileType[_UIInventoryItems.Length]; 
        int count = 0; 
        foreach (UIOnSelection UIInventoryItem in _UIInventoryItems)
        {
            inventoryItems[count] = UIInventoryItem.GetInventoryItemPrefab().GetComponent<Tile>().tileType;
            count++;
        }
        return inventoryItems; 
    } */

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
