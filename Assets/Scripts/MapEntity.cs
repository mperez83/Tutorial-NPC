using UnityEngine;

public abstract class MapEntity : MonoBehaviour
{
    public bool isEnemy;
    public abstract void Init(MapHandlerExp mapHandler, Vector2 initSpace);
    public abstract void OnMapActivate();
    public abstract void OnMapUpdate(float actionTimer, float actionTimerLength);
    public abstract void OnMapAction();
}