using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandlerExp : MonoBehaviour
{
    public GameObject levelTilemap;
    public GameObject floorTilemap;
    public Tile[,] mapGrid;
    PlayerHandler playerHandler;



    void Start()
    {
        playerHandler = GetComponent<PlayerHandler>();

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

        mapGrid = new Tile[mapWidth, mapHeight];

        //Now, populate the mapGrid, keeping what the offset of each tile should be in mind
        foreach (Transform child in levelTilemap.transform)
        {
            child.Translate(new Vector2(xOffset, yOffset));

            int x = (int)child.position.x;
            int y = (int)child.position.y;

            //If we're looking at the hero, do all the necessary hero setup
            if (child.name == "Hero")
            {
                playerHandler.player = child.gameObject;
                playerHandler.mapGrid = mapGrid;

                //Top player spawn
                if (y == (mapGrid.GetLength(1) - 1))
                {
                    playerHandler.playerSpace = new Vector2(x, y + 4);
                    playerHandler.nextSpace = new Vector2(x, y + 3);
                    playerHandler.playerDir = PlayerHandler.PlayerDirections.Down;
                }

                //Bottom player spawn
                else if (y == 0)
                {
                    playerHandler.playerSpace = new Vector2(x, y - 4);
                    playerHandler.nextSpace = new Vector2(x, y - 3);
                    playerHandler.playerDir = PlayerHandler.PlayerDirections.Up;
                }

                //Left player spawn
                else if (x == 0)
                {
                    playerHandler.playerSpace = new Vector2(x - 4, y);
                    playerHandler.nextSpace = new Vector2(x - 3, y);
                    playerHandler.playerDir = PlayerHandler.PlayerDirections.Right;
                }

                //Right player spawn
                else if (x == (mapGrid.GetLength(0) - 1))
                {
                    playerHandler.playerSpace = new Vector2(x + 4, y);
                    playerHandler.nextSpace = new Vector2(x + 3, y);
                    playerHandler.playerDir = PlayerHandler.PlayerDirections.Left;
                }

                //Else illegal spawn
                else
                {
                    print("Tried to spawn player at x=" + x + ", y=" + y + " which is ILLEGAL");
                }
            }

            //Otherwise, just slap it in the mapGrid
            else
            {
                mapGrid[x, y] = child.GetComponent<Tile>();
            }
        }

        //Lastly, offset the floor tilemap so it aligns with everything else
        floorTilemap.transform.Translate(new Vector2(xOffset, yOffset));
    }
}