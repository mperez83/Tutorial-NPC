using UnityEngine;
using Cinemachine;

public class PixelColorMapHandler : MonoBehaviour
{
    /*public Texture2D map;
    public ColorToPrefab[] colorMappings;
    public CinemachineVirtualCamera vcam;
    public Tile[,] mapGrid;
    HeroHandler heroHandler;


    private void Start()
    {
        heroHandler = GetComponent<HeroHandler>();
        GenerateLevel();
    }



    void GenerateLevel()
    {
        mapGrid = new Tile[map.width, map.height];

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
                    heroHandler.Init(mapGrid, new Vector2(x, y));
                }

                //Otherwise, just do a normal spawn
                else
                {
                    mapGrid[x, y] = Instantiate(colorMapping.prefab, spawnPos, Quaternion.identity, transform).GetComponent<Tile>();
                }
            }
        }
    }*/
}