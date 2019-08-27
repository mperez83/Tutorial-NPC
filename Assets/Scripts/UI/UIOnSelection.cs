using System;
using UnityEngine;
using UnityEngine.UI;

public class UIOnSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;   
    private GameObject prefabToSpawnClone; 
    private bool placingInventoryItem; 
    private MapHandler mapHandler; 

    private void Awake() 
    {
        mapHandler = FindObjectOfType<MapHandler>();     
    }

    private void LateUpdate()  
    {
        if (placingInventoryItem)
        {
            GameMaster.Instance.InventoryItemSelected(); 
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
            GameMaster.Instance.InventoryItemDeselected(); 
            Cursor.visible = true; 
        }
    }

    private void PlaceInventoryItemDown(Vector3 mousePosition, GameObject inventoryItem)
    {   
        Vector3 positionToPlaceItem = mousePosition.RoundXAndYCoords(); 
        inventoryItem.transform.position = positionToPlaceItem; 
        placingInventoryItem = false; 
    }

    public void OnUIElementSelected()
    {
        if (!placingInventoryItem)
        {
            placingInventoryItem = true; 
            prefabToSpawnClone = Instantiate(prefabToSpawn, transform.position, 
                Quaternion.identity, mapHandler.transform);
        }
    }
}
