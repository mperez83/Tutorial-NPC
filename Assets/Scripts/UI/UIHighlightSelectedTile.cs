﻿using UnityEngine;

public class UIHighlightSelectedTile : MonoBehaviour
{
    [SerializeField]
    private GameObject tileHighlightImage; 
    private GameObject tileHighlightImageClone; 
    private bool tileHighlightInstantiated;

    private MapHandlerExp mapHandlerExp;

    private void Start() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        GameMaster.Instance.OnInventoryItemSelected += HighlightSelectedTile;  
        GameMaster.Instance.OnInventoryItemDeselected += DeleteHighlightPrefab;
    }

    public void HighlightSelectedTile()
    { 
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0; 

        screenPoint = screenPoint.RoundXAndYCoords(); 

        if (!tileHighlightInstantiated)
        {
            tileHighlightImageClone = Instantiate(tileHighlightImage, screenPoint, 
                Quaternion.identity, transform);
            tileHighlightInstantiated = true; 
        }
        if (mapHandlerExp.mapGrid[(int)screenPoint.x, (int)screenPoint.y] == null)
            tileHighlightImageClone.transform.position = screenPoint.RoundXAndYCoords(); 
    }

    public void DeleteHighlightPrefab()
    {
        tileHighlightInstantiated = false; 
        Destroy(tileHighlightImageClone); 
    }

    private void OnDestroy() 
    {
        GameMaster.Instance.OnInventoryItemSelected -= HighlightSelectedTile;
        GameMaster.Instance.OnInventoryItemDeselected -= DeleteHighlightPrefab;
    }
}