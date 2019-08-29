using System.Collections.Generic;
using UnityEngine;

public class HeroHandler : MapEntity
{
    Tile[,] tileGrid;
    MapEntity[,] entityGrid;

    public enum HeroStates { Entering, Moving, Frustrated, SawPot, Victory };
    HeroStates heroState = HeroStates.Entering;

    public enum HeroDirections { Up, Down, Left, Right };
    HeroDirections heroDir;

    Vector2 curSpace;
    Vector2 nextSpace;

    List<string> inventory = new List<string>();

    public GameObject visibleLadderPrefab;
    GameObject visibleLadder;



    public override void Init(ref Tile[,] tileGrid, ref MapEntity[,] entityGrid, Vector2 initSpace)
    {
        this.tileGrid = tileGrid;
        this.entityGrid = entityGrid;

        //Top player spawn
        if (curSpace.y == (tileGrid.GetLength(1) - 1))
        {
            heroDir = HeroDirections.Down;
            curSpace = new Vector2(initSpace.x, initSpace.y + 2);
            nextSpace = new Vector2(curSpace.x, curSpace.y - 1);
        }

        //Bottom player spawn
        else if (curSpace.y == 0)
        {
            heroDir = HeroDirections.Up;
            curSpace = new Vector2(initSpace.x, initSpace.y - 2);
            nextSpace = new Vector2(curSpace.x, curSpace.y + 1);
        }

        //Left player spawn
        else if (curSpace.x == 0)
        {
            heroDir = HeroDirections.Right;
            curSpace = new Vector2(initSpace.x - 2, initSpace.y);
            nextSpace = new Vector2(curSpace.x + 1, curSpace.y);
        }

        //Right player spawn
        else if (curSpace.x == (tileGrid.GetLength(0) - 1))
        {
            heroDir = HeroDirections.Left;
            curSpace = new Vector2(initSpace.x + 2, initSpace.y);
            nextSpace = new Vector2(curSpace.x - 1, curSpace.y);
        }

        //Else illegal spawn
        else
        {
            print("Tried to spawn the hero at x=" + curSpace.x + ", y=" + curSpace.y + " which is ILLEGAL (so we're just gonna default to some arbitrary values, please fix)");
            heroDir = HeroDirections.Up;
            nextSpace = new Vector2(curSpace.x, curSpace.y + 1);
        }
    }



    public override void MapUpdate(float actionTimer, float actionTimerLength)
    {
        float actualLerpValue;  //Declared up here so we don't deal with "already declared" bullshit inside the switch statement

        switch (heroState)
        {
            case HeroStates.Entering:
                actualLerpValue = actionTimer / actionTimerLength;
                transform.position = Vector2.Lerp(curSpace, nextSpace, actualLerpValue);
                break;

            case HeroStates.Moving:
                actualLerpValue = actionTimer / actionTimerLength;
                transform.position = Vector2.Lerp(curSpace, nextSpace, actualLerpValue);
                break;

            case HeroStates.Frustrated:
                //Maybe do something here?
                break;

            case HeroStates.SawPot:
                //Not sure if anything should go here
                break;

            //Like HeroStates.Moving, except we don't use the SetNextSpace function
            case HeroStates.Victory:
                actualLerpValue = actionTimer / actionTimerLength;
                transform.position = Vector2.Lerp(curSpace, nextSpace, actualLerpValue);
                break;
        }

    }



    public override void MapAction()
    {
        switch (heroState)
        {
            case HeroStates.Entering:
                curSpace.x = nextSpace.x;
                curSpace.y = nextSpace.y;

                switch (heroDir)
                {
                    case HeroDirections.Up:
                        nextSpace.y = nextSpace.y + 1;
                        break;
                    case HeroDirections.Down:
                        nextSpace.y = nextSpace.y - 1;
                        break;
                    case HeroDirections.Left:
                        nextSpace.x = nextSpace.x - 1;
                        break;
                    case HeroDirections.Right:
                        nextSpace.x = nextSpace.x + 1;
                        break;
                }

                //When the player enters the tileGrid, set them to HeroStates.Moving
                if (nextSpace.x >= 0 && nextSpace.y >= 0 && nextSpace.x < tileGrid.GetLength(0) && nextSpace.y < tileGrid.GetLength(1))
                {
                    heroState = HeroStates.Moving;
                }
                break;

            case HeroStates.Moving:
                curSpace.x = nextSpace.x;
                curSpace.y = nextSpace.y;

                //If the hero is over a tile, do something
                if (tileGrid[(int)curSpace.x, (int)curSpace.y]) HandleCurrentTile();
                else
                {
                    if (visibleLadder != null) Destroy(visibleLadder);  //Really bad, kinda hope we can move this elsewhere at some point
                }

                //If the hero is over an entity, do something
                if (entityGrid[(int)curSpace.x, (int)curSpace.y]) HandleCurrentEntity();

                SetNextSpace();
                break;

            case HeroStates.Frustrated:
                switch (heroDir)
                {
                    case HeroDirections.Up:
                        heroDir = HeroDirections.Down;
                        break;
                    case HeroDirections.Down:
                        heroDir = HeroDirections.Up;
                        break;
                    case HeroDirections.Left:
                        heroDir = HeroDirections.Right;
                        break;
                    case HeroDirections.Right:
                        heroDir = HeroDirections.Left;
                        break;
                }
                heroState = HeroStates.Moving;
                SetNextSpace();
                break;

            case HeroStates.SawPot:
                break;

            case HeroStates.Victory:
                //GameMaster.Instance.AdvanceLevel();
                curSpace.x = nextSpace.x;
                curSpace.y = nextSpace.y;

                switch (heroDir)
                {
                    case HeroDirections.Up:
                        nextSpace.y = nextSpace.y + 1;
                        break;
                    case HeroDirections.Down:
                        nextSpace.y = nextSpace.y - 1;
                        break;
                    case HeroDirections.Left:
                        nextSpace.x = nextSpace.x - 1;
                        break;
                    case HeroDirections.Right:
                        nextSpace.x = nextSpace.x + 1;
                        break;
                }
                break;
        }
    }





    //If something is at the hero's space, decide what to do (e.g. change direction, pick up item, etc)
    void HandleCurrentTile()
    {
        int curX = (int)curSpace.x;
        int curY = (int)curSpace.y;

        Tile checkTile = tileGrid[curX, curY].GetComponent<Tile>();

        switch (checkTile.tileType)
        {
            case Tile.TileType.TurnUp:
                heroDir = HeroDirections.Up;
                break;

            case Tile.TileType.TurnDown:
                heroDir = HeroDirections.Down;
                break;

            case Tile.TileType.TurnLeft:
                heroDir = HeroDirections.Left;
                break;

            case Tile.TileType.TurnRight:
                heroDir = HeroDirections.Right;
                break;

            case Tile.TileType.Pit:
                if (!inventory.Contains("Ladder"))
                {
                    print("Aaaaahh!! The hero fell!!!");
                }
                break;

            case Tile.TileType.Ladder:
                if (!inventory.Contains("Ladder")) inventory.Add("Ladder");
                Destroy(tileGrid[curX, curY].gameObject);
                break;

            case Tile.TileType.Sword:
                if (!inventory.Contains("Sword")) inventory.Add("Sword");
                Destroy(tileGrid[curX, curY].gameObject);
                break;
        }

        //Cheeky check to see if we should delete a visible ladder
        if (visibleLadder != null)
        {
            if (checkTile.tileType != Tile.TileType.Pit) Destroy(visibleLadder);
        }

    }



    void HandleCurrentEntity()
    {
        int curX = (int)curSpace.x;
        int curY = (int)curSpace.y;

        MapEntity checkEntity = entityGrid[curX, curY].GetComponent<MapEntity>();
        if (checkEntity.isEnemy)
        {
            if (inventory.Contains("Sword"))
            {
                Destroy(entityGrid[curX, curY].gameObject);
            }
            else
            {
                heroState = HeroStates.SawPot;  //Should probably be a death state
            }
        }
    }



    //Look ahead to see if there's something the hero has to deal with (e.g. walking out of bounds, running into an impassable tile, etc)
    void SetNextSpace()
    {
        int curX = (int)curSpace.x;
        int curY = (int)curSpace.y;

        if (CheckForLineOfSightWithPot(curX, curY))
        {
            print("THE HERO SAW THE POT");
            heroState = HeroStates.SawPot;
            Destroy(this);
            return;
        }

        int nextX = curX;
        int nextY = curY;

        switch (heroDir)
        {
            case HeroDirections.Up:
                nextY = curY + 1;
                break;

            case HeroDirections.Down:
                nextY = curY - 1;
                break;

            case HeroDirections.Left:
                nextX = curX - 1;
                break;

            case HeroDirections.Right:
                nextX = curX + 1;
                break;
        }

        if (nextX < 0 || nextX >= tileGrid.GetLength(0) || nextY < 0 || nextY >= tileGrid.GetLength(1))
        {
            //print("Out of bounds!!");
            heroState = HeroStates.Frustrated;
        }

        //If the space we're looking at isn't null, figure out what it is and what we should do about it
        else if (tileGrid[nextX, nextY] != null)
        {
            HandleTileOnNextSpace(nextX, nextY);
        }

        //If the space we're looking at is null, just move forward
        else
        {
            //print("All clear to keep moving!");
            nextSpace.x = nextX;
            nextSpace.y = nextY;
        }
    }

    void HandleTileOnNextSpace(int nextX, int nextY)
    {
        switch (tileGrid[nextX, nextY].GetComponent<Tile>().tileType)
        {
            case Tile.TileType.Pit:
                if (!inventory.Contains("Ladder"))
                {
                    //print("Can't pass!!!");
                    heroState = HeroStates.Frustrated;
                }
                else
                {
                    int secondX = nextX;
                    int secondY = nextY;
                    bool horizontal = false;

                    switch (heroDir)
                    {
                        case HeroDirections.Up:
                            secondY = nextY + 1;
                            horizontal = false;
                            break;

                        case HeroDirections.Down:
                            secondY = nextY - 1;
                            horizontal = false;
                            break;

                        case HeroDirections.Left:
                            secondX = nextX - 1;
                            horizontal = true;
                            break;

                        case HeroDirections.Right:
                            secondX = nextX + 1;
                            horizontal = true;
                            break;
                    }

                    if (secondX < 0 || secondX >= tileGrid.GetLength(0) || secondY < 0 || secondY >= tileGrid.GetLength(1))
                    {
                        //print("Out of bounds, so it's ok if the player crosses with a ladder because they'll bump into a wall");
                        nextSpace.x = nextX;
                        nextSpace.y = nextY;

                        if (visibleLadder != null) Destroy(visibleLadder);
                        visibleLadder = Instantiate(visibleLadderPrefab, new Vector2(nextSpace.x, nextSpace.y), Quaternion.identity);
                        if (horizontal) visibleLadder.transform.Rotate(new Vector3(0, 0, 90));
                    }
                    else
                    {
                        if (tileGrid[secondX, secondY] != null && tileGrid[secondX, secondY].GetComponent<Tile>().tileType == Tile.TileType.Pit)
                        {
                            //print("The gap is too wide to cross!!!");
                            heroState = HeroStates.Frustrated;
                        }
                        else
                        {
                            //print("You used your ladder to cross the gap!!");
                            nextSpace.x = nextX;
                            nextSpace.y = nextY;

                            if (visibleLadder != null) Destroy(visibleLadder);
                            visibleLadder = Instantiate(visibleLadderPrefab, new Vector2(nextSpace.x, nextSpace.y), Quaternion.identity);
                            if (horizontal) visibleLadder.transform.Rotate(new Vector3(0, 0, 90));
                        }
                    }
                }
                break;

            case Tile.TileType.Exit:
                print("You won!!!");
                heroState = HeroStates.Victory;

                nextSpace.x = nextX;
                nextSpace.y = nextY;

                //This part is inefficient, this could be better

                //Top player exit
                if (nextSpace.y == (tileGrid.GetLength(1) - 1))
                    heroDir = HeroDirections.Up;

                //Bottom player exit
                else if (curSpace.y == 0)
                    heroDir = HeroDirections.Down;

                //Left player exit
                else if (curSpace.x == 0)
                    heroDir = HeroDirections.Left;

                //Right player exit
                else if (curSpace.x == (tileGrid.GetLength(0) - 1))
                    heroDir = HeroDirections.Right;

                else
                    print("??? the exit was not along the edge of the map");
                
                break;

            default:
                //print("All clear to keep moving!");
                nextSpace.x = nextX;
                nextSpace.y = nextY;
                break;
        }
    }





    Tile GetTileAtCoords(int x, int y)
    {
        if (x < 0 || x >= tileGrid.GetLength(0) || y < 0 || y >= tileGrid.GetLength(1))
            return null;
        else
            return tileGrid[x, y].GetComponent<Tile>();
    }



    bool CheckForLineOfSightWithPot(int curX, int curY)
    {
        switch (heroDir)
        {
            case HeroDirections.Up:
                for (int i = curY; i < (tileGrid.GetLength(1) - 1); i++)
                {
                    Tile checkTile = tileGrid[curX, i];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Down:
                for (int i = curY; i > 0; i--)
                {
                    Tile checkTile = tileGrid[curX, i];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Left:
                for (int i = curX; i > 0; i--)
                {
                    Tile checkTile = tileGrid[i, curY];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Right:
                for (int i = curX; i < (tileGrid.GetLength(1) - 1); i++)
                {
                    Tile checkTile = tileGrid[i, curY];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
        }

        return false;
    }
}