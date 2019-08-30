using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    private Tile tile; 
    private int count = 0;

    private void Awake() 
    {
        tile = GetComponent<Tile>();
    }

    private void ChangeArrowDirection()
    {

        if (count != 3)
            count++;
        else
            count = 0;

        switch(count)
        {
            case 0:
                tile.tileType = Tile.TileType.TurnUp; 
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break; 
            case 1:
                tile.tileType = Tile.TileType.TurnRight; 
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break;
            case 2: 
                tile.tileType = Tile.TileType.TurnDown; 
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break;
            case 3:
                tile.tileType = Tile.TileType.TurnLeft;
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break; 
        }
        // tile.TileType = next item in ArrowDirections
    }
}
