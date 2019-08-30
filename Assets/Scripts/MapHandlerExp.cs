using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapHandlerExp : MonoBehaviour
{
    bool mapActive = false;

    [Range(0.1f, 1f)]
    public float actionTimerLength;
    float actionTimer = 0;

    Tile[,] tileGrid;
    public GameObject levelTilemap;
    public GameObject floorTilemap;

    public CinemachineVirtualCamera vcam;

    MapEntity[,] entityGrid;
    public HeroHandler heroHandler;

    [HideInInspector]
    public List<MapEntity> enemies = new List<MapEntity>();



    void Start()
    {
        float topBound = levelTilemap.transform.GetChild(0).transform.position.y;
        float bottomBound = levelTilemap.transform.GetChild(0).transform.position.y;
        float leftBound = levelTilemap.transform.GetChild(0).transform.position.x;
        float rightBound = levelTilemap.transform.GetChild(0).transform.position.x;

        //First, scrub through the entire tilemap to determine which tile is at the bottom left corner
        foreach (Transform child in levelTilemap.transform)
        {
            if (child.position.x < leftBound)
                leftBound = child.position.x;
            else if (child.position.x > rightBound)
                rightBound = child.position.x;

            if (child.position.y < bottomBound)
                bottomBound = child.position.y;
            else if (child.position.y > topBound)
                topBound = child.position.y;
        }

        int mapWidth = (int)(rightBound - leftBound) + 1;
        int mapHeight = (int)(topBound - bottomBound) + 1;

        float xOffset = Mathf.Abs(leftBound);
        float yOffset = Mathf.Abs(bottomBound);

        tileGrid = new Tile[mapWidth, mapHeight];
        entityGrid = new MapEntity[mapWidth, mapHeight];

        //Now, populate the tileGrid, keeping what the offset of each tile should be in mind
        foreach (Transform child in levelTilemap.transform)
        {
            child.Translate(new Vector2(xOffset, yOffset));

            int x = (int)child.position.x;
            int y = (int)child.position.y;

            //Check if an entity is at this space
            MapEntity entityCheck = child.GetComponent<MapEntity>();
            if (entityCheck)
            {
                entityCheck.Init(ref tileGrid, ref entityGrid, new Vector2(x, y));
                if (child.name != "Hero") enemies.Add(entityCheck);
                entityGrid[x, y] = entityCheck;
            }

            //Check if a tile is at this space
            Tile tileCheck = child.GetComponent<Tile>();
            if (tileCheck)
            {
                tileGrid[x, y] = tileCheck;
            }
        }

        //Lastly, offset the floor tilemap so it aligns with everything else
        floorTilemap.transform.Translate(new Vector2(xOffset, yOffset));

        //Lastly lastly, loop through all pit tiles and set their graphics depending on if there are adjacent pits
        GetComponent<PitConnector>().ConnectAllPits(tileGrid);
    }



    void Update()
    {
        if (mapActive)
        {
            //Update the enemies
            foreach (MapEntity enemy in enemies)
                if (enemy) enemy.MapUpdate(actionTimer, actionTimerLength);

            //Update the player
            if (heroHandler) heroHandler.MapUpdate(actionTimer, actionTimerLength);

            actionTimer += Time.deltaTime;
            if (actionTimer >= actionTimerLength)
            {
                actionTimer = 0;    //This could probably be tweaked to subtract from timer, rather than setting it to zero, allowing multiple actions per frame if the timer is short enough

                //Update the enemies
                foreach (MapEntity enemy in enemies)
                    if (enemy) enemy.MapUpdate(actionTimer, actionTimerLength);

                //Update the hero
                if (heroHandler) heroHandler.MapAction();
            }
        }
    }



    public void ActivateMap()
    {
        if (!mapActive)
        {
            mapActive = true;
            heroHandler.enabled = true;
        }
    }



    public Tile[,] GetTileGrid()
    {
        return tileGrid;
    }

    public bool GetIfTileExistsInLocation(int x, int y)
    {
        if (tileGrid[x,y] != null)
            return true; 
        else
            return false; 
    }

    public bool GetIfInsideTileGrid(int x, int y)
    {
        if (x >= 0 && x < tileGrid.GetLength(0) && y >= 0 && y < tileGrid.GetLength(1))
            return true;
        else
            return false;
    }

    public MapEntity[,] GetEntityGrid()
    {
        return entityGrid;
    }
}