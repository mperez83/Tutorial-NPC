using System;
using TMPro;
using UnityEngine;

public class NumberOfInventoryItemsController : MonoBehaviour
{

    [SerializeField]
    private int numOfItemInInventory; 
    private TextMeshProUGUI tmproText; 
    private bool addedOneToInventory;

    public int NumOfItemInInventory { get {return numOfItemInInventory;} }

    private void Start() 
    {
        GameMaster.Instance.OnInventoryItemAdded += ChangeNumOfInventoryItems;
        GameMaster.Instance.OnInventoryItemSelected += AddInventoryItem;

        if (numOfItemInInventory < 0)
            numOfItemInInventory = 0;      

        tmproText = GetComponentInChildren<TextMeshProUGUI>(); 
        tmproText.text = numOfItemInInventory.ToString(); 
    }

    private void OnDestroy() 
    {
        GameMaster.Instance.OnInventoryItemAdded -= ChangeNumOfInventoryItems; 
        GameMaster.Instance.OnInventoryItemSelected -= AddInventoryItem;     
    }

    private void AddInventoryItem(GameObject itemSelected)
    {
        if (gameObject == itemSelected && addedOneToInventory == false)
        {
            if (GameMaster.Instance.TileMapInventoryItemSelected == true)
            {
                numOfItemInInventory++; 
                tmproText.text = numOfItemInInventory.ToString();
                addedOneToInventory = true;
                Debug.Log("numofitem addition called");
            }
        }
    }

    private void ChangeNumOfInventoryItems(GameObject inventoryItemController)
    {
        if (gameObject == inventoryItemController)
        {
            if (numOfItemInInventory > 0 && 
                GameMaster.Instance.TileMapInventoryItemSelected)
                {
                    numOfItemInInventory--;  
                    tmproText.text = numOfItemInInventory.ToString(); 
                }
            addedOneToInventory = false;
        } 
    }
}
