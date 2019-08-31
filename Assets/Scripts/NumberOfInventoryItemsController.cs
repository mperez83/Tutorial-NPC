using TMPro;
using UnityEngine;

public class NumberOfInventoryItemsController : MonoBehaviour
{

    [SerializeField]
    private int numOfItemInInventory; 
    private TextMeshProUGUI tmproText; 

    public int NumOfItemInInventory { get {return numOfItemInInventory;} }

    private void Start() 
    {
        GameMaster.instance.OnInventoryItemAdded += ChangeNumOfInventoryItems;

        if (numOfItemInInventory < 0)
            numOfItemInInventory = 0;      

        tmproText = GetComponentInChildren<TextMeshProUGUI>(); 
        tmproText.text = numOfItemInInventory.ToString(); 
    }

    private void OnDestroy() 
    {
        GameMaster.instance.OnInventoryItemAdded -= ChangeNumOfInventoryItems;     
    }

    private void ChangeNumOfInventoryItems(GameObject inventoryItemController)
    {
        if (gameObject == inventoryItemController)
        {
            if (numOfItemInInventory > 0 && 
                GameMaster.instance.TileMapInventoryItemSelected)
                {
                    numOfItemInInventory--;  
                    tmproText.text = numOfItemInInventory.ToString(); 
                }
            else if (GameMaster.instance.TileMapInventoryItemSelected == false)
                {
                    numOfItemInInventory++;
                    tmproText.text = numOfItemInInventory.ToString();
                }
        } 
    }
}
