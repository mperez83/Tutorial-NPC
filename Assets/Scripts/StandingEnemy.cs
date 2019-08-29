using UnityEngine;

public class StandingEnemy : MapEntity
{
    Tile[,] tileGrid;
    MapEntity[,] entityGrid;

    Vector2 curSpace;

    float mainDeg;
    float sinVal;
    float cosVal;



    public override void Init(ref Tile[,] tileGrid, ref MapEntity[,] entityGrid, Vector2 initSpace)
    {
        this.tileGrid = tileGrid;
        this.entityGrid = entityGrid;
        curSpace = initSpace;
        mainDeg = Random.Range(0f, 360f);
    }

    public override void MapUpdate(float actionTimer, float actionTimerLength)
    {
        //mainDeg = 360 * (actionTimer / actionTimerLength);
        mainDeg += (360 * (1 / 6f)) * Time.deltaTime;
        if (mainDeg > 360) mainDeg = mainDeg - 360;

        sinVal = 0.1f * Mathf.Sin((mainDeg * 2) * Mathf.Deg2Rad);
        cosVal = 0.2f * Mathf.Sin(mainDeg * Mathf.Deg2Rad);

        transform.position = new Vector2(curSpace.x + cosVal, curSpace.y + sinVal);
    }

    public override void MapAction()
    {

    }
}