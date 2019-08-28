using UnityEngine;

public class NumberOfInventoryItemsController : MonoBehaviour
{

    [SerializeField]
    private int numOfItemInInventory; 

    public int NumOfItemInInventory { get {return numOfItemInInventory;} }

    private void Start() 
    {
        GameMaster.Instance.OnInventoryItemAdded += DecrementNumOfInventoryItems;

        if (numOfItemInInventory < 0)
            numOfItemInInventory = 0;      
    }

    private void OnDestroy() 
    {
        GameMaster.Instance.OnInventoryItemAdded -= DecrementNumOfInventoryItems;     
    }

    private void DecrementNumOfInventoryItems(GameObject inventoryItemController)
    {
        if (gameObject == inventoryItemController)
        {
            if (numOfItemInInventory > 0)
                numOfItemInInventory--;  
        } 
    }
}
