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
        GameMaster.Instance.OnInventoryItemAdded += DecrementNumOfInventoryItems;

        if (numOfItemInInventory < 0)
            numOfItemInInventory = 0;      
        tmproText = GetComponentInChildren<TextMeshProUGUI>(); 
        tmproText.text = numOfItemInInventory.ToString(); 
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
                {
                    numOfItemInInventory--;  
                    tmproText.text = numOfItemInInventory.ToString(); 
                }
        } 
    }
}
