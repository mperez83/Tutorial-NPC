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

                if (colorMapping.color.r == 1 && colorMapping.color.b == 1)
                {
                    mapHandler.player = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform);
                    mapHandler.playerSpace = new Vector2(x, y);
                    mapHandler.nextSpace = new Vector2(x, y + 1);
                }
                else
                {
                    mapHandler.mapGrid[x, y] = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform);
                }
            }
        }
    }
}