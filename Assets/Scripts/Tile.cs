using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Impassable, TurnUp, TurnDown, TurnLeft, TurnRight };
    public TileType tileType;
}