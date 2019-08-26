using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Texture2D map;
    public ColorToPrefab[] colorMappings;
    MapHandler mapHandler;



    void Start()
    {
        mapHandler = GetComponent<MapHandler>();
        GenerateLevel();
    }

    void GenerateLevel()
    {
        mapHandler.mapGrid = new GameObject[map.width, map.height];

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
                    mapHandler.player = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform);
                    mapHandler.playerSpace = new Vector2(x, y);
                    mapHandler.nextSpace = new Vector2(x, y + 1);

                    Vector3 playerPos = mapHandler.player.transform.position;
                    Camera.main.transform.position = new Vector3(playerPos.x, playerPos.y, Camera.main.transform.position.z);
                    float camPosY = ((GameMaster.instance.GetCamTopEdge() - GameMaster.instance.GetCamBottomEdge()) / 2f) - 2;
                    Camera.main.transform.position = new Vector3(playerPos.x, camPosY, Camera.main.transform.position.z);
                }

                //Otherwise, just do a normal spawn
                else
                {
                    mapHandler.mapGrid[x, y] = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform);
                }
            }
        }
    }
}