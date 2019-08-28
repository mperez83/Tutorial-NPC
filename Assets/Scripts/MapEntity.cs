using UnityEngine;

public abstract class MapEntity : MonoBehaviour
{
    public bool isEnemy;
    public abstract void Init(ref Tile[,] tileGrid, ref MapEntity[,] entityGrid, Vector2 initSpace);
    public abstract void MapUpdate(float actionTimer, float actionTimerLength);
    public abstract void MapAction();
}