using UnityEngine;

public abstract class MapEntity : MonoBehaviour
{
    public abstract void Init(Tile[,] mapGrid, Vector2 initSpace);
    public abstract void MapUpdate(float actionTimer, float actionTimerLength);
    public abstract void MapAction();
}