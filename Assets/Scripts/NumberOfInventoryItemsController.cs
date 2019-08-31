using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfInventoryItemsController : MonoBehaviour
{

    [SerializeField]
    private int numOfItemInInventory;
    private TextMeshProUGUI tmproText;
    private bool addedOneToInventory;
    private Button button; 

    public int NumOfItemInInventory { get { return numOfItemInInventory; } }

    private void Start()
    {
        GameMaster.instance.OnInventoryItemAdded += ChangeNumOfInventoryItems;
        GameMaster.instance.OnInventoryItemSelected += AddInventoryItem;

        if (numOfItemInInventory < 0)
            numOfItemInInventory = 0;

        tmproText = GetComponentInChildren<TextMeshProUGUI>();
        tmproText.text = numOfItemInInventory.ToString();

        button = GetComponent<Button>();
    }

    private void OnDestroy()
    {
        GameMaster.instance.OnInventoryItemAdded -= ChangeNumOfInventoryItems;
        GameMaster.instance.OnInventoryItemSelected -= AddInventoryItem;
    }

    private void AddInventoryItem(GameObject itemSelected)
    {
        if (gameObject == itemSelected && addedOneToInventory == false)
        {
            if (GameMaster.instance.TileMapInventoryItemSelected == true)
            {
                numOfItemInInventory++;
                tmproText.text = numOfItemInInventory.ToString();
                button.interactable = true; 
                addedOneToInventory = true;
            }
        }
    }

    private void ChangeNumOfInventoryItems(GameObject inventoryItemController)
    {
        if (gameObject == inventoryItemController)
        {
            if (numOfItemInInventory > 0)
            {
                numOfItemInInventory--;
                tmproText.text = numOfItemInInventory.ToString();
            }
            addedOneToInventory = false;
        }
    }
}
