using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;
    public string[] levels;
    public int currentLevel;
    public bool TileMapInventoryItemSelected; 

    public event Action<GameObject> OnInventoryItemSelected;
    public event Action OnInventoryItemDeselected;
    public event Action<GameObject> OnInventoryItemAdded;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else Instance = this;
        DontDestroyOnLoad(this);
    }

    public void AdvanceLevel()
    {
        currentLevel++;
        if (currentLevel > levels.Length) currentLevel = 1;
        SceneManager.LoadScene(levels[currentLevel - 1]);
    }



    internal void InventoryItemSelected(GameObject inventoryItem)
    {
        if (OnInventoryItemSelected != null)
            OnInventoryItemSelected(inventoryItem); 
    }

    internal void InventoryItemDeselected()
    {
        if (OnInventoryItemSelected != null)
            OnInventoryItemDeselected(); 

        TileMapInventoryItemSelected = false; 
    }

    internal void AddInventoryItemToMap(GameObject inventoryItemController)
    {
        if (OnInventoryItemAdded != null)
            OnInventoryItemAdded(inventoryItemController); 
    }



    public float GetCamTopEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -(Camera.main.transform.position.z))).y;
    }

    public float GetCamBottomEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).y;
    }

    public float GetCamLeftEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).x;
    }

    public float GetCamRightEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -(Camera.main.transform.position.z))).x;
    }
}