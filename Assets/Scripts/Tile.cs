using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Impassable, Pot, TurnUp, TurnDown, TurnLeft, TurnRight, Exit };
    public TileType tileType;
    public bool blocksVision;
}