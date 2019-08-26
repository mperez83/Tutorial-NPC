using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public Texture2D map;
    public ColorToPrefab[] colorMappings;
    public GameObject[,] mapGrid;
    PlayerHandler playerHandler;



    void Start()
    {
        playerHandler = GetComponent<PlayerHandler>();
        GenerateLevel();
    }



    void GenerateLevel()
    {
        mapGrid = new GameObject[map.width, map.height];

        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {
                GenerateTile(i, j);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector2 spawnPos = new Vector2(x, y);

                //If we're spawning the player, do some special stuff
                if (colorMapping.color.r == 1 && colorMapping.color.b == 1)
                {
                    playerHandler.player = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform);
                    playerHandler.mapGrid = mapGrid;

                    //Top player spawn
                    if (y == (map.height - 1))
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
                    else if (x == (map.width - 1))
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


                    Vector3 playerPos = playerHandler.player.transform.position;
                    Camera.main.transform.position = new Vector3(playerPos.x, playerPos.y, Camera.main.transform.position.z);
                    float camPosY = ((GameMaster.instance.GetCamTopEdge() - GameMaster.instance.GetCamBottomEdge()) / 2f) - 2;
                    Camera.main.transform.position = new Vector3(playerPos.x, camPosY, Camera.main.transform.position.z);
                }

                //Otherwise, just do a normal spawn
                else
                {
                    mapGrid[x, y] = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform);
                }
            }
        }
    }
}