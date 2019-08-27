using System;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public event Action OnInventoryItemSelected;
    public event Action OnInventoryItemDeselected;

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

    /* internal void InventoryItemSelected()
    {
        if (OnInventoryItemSelected != null)
            InventoryItemSelected(); 
    }

    internal void InventoryItemDeselected()
    {
        if (OnInventoryItemSelected != null)
            InventoryItemDeselected(); 
    } */

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