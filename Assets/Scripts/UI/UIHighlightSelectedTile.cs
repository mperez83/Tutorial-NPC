using UnityEngine;

public class UIHighlightSelectedTile : MonoBehaviour
{
    [SerializeField]
    private GameObject tileHighlightImage = null; 
    private GameObject tileHighlightImageClone;
    SpriteRenderer tileHighlightSpriteRenderer;

    private MapHandlerExp mapHandlerExp;

    private void Start() 
    {
        mapHandlerExp = FindObjectOfType<MapHandlerExp>();
        GameMaster.instance.OnInventoryItemSelected += HighlightSelectedTile;  
        GameMaster.instance.OnInventoryItemDeselected += DeleteHighlightPrefab;

        tileHighlightImageClone = Instantiate(tileHighlightImage, Vector2.zero, Quaternion.identity, transform);
        tileHighlightSpriteRenderer = tileHighlightImageClone.GetComponent<SpriteRenderer>();
        tileHighlightSpriteRenderer.enabled = false;
    }

    public void HighlightSelectedTile(GameObject placeholderParameter)
    {
        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int x = Mathf.RoundToInt(screenPoint.x);
        int y = Mathf.RoundToInt(screenPoint.y);

        if (mapHandlerExp.GetIfInsideTileGrid(x, y) && mapHandlerExp.tileGrid[x, y] == null)
        {
            tileHighlightSpriteRenderer.enabled = true;
            tileHighlightImageClone.transform.position = new Vector2(x, y);
        }
        else
        {
            tileHighlightSpriteRenderer.enabled = false;
        }
    }

    public void DeleteHighlightPrefab()
    {
        tileHighlightSpriteRenderer.enabled = false;
    }

    private void OnDestroy() 
    {
        GameMaster.instance.OnInventoryItemSelected -= HighlightSelectedTile;
        GameMaster.instance.OnInventoryItemDeselected -= DeleteHighlightPrefab;
    }
}
