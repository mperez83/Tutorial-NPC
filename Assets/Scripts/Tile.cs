using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Impassable, TurnUp, TurnDown, TurnLeft, TurnRight, Exit };
    public TileType tileType;
}