using System.Collections.Generic;
using UnityEngine;

public class HeroHandler : MapEntity
{
    Tile[,] mapGrid;

    public enum HeroStates { Entering, Moving, Frustrated, SawPot, Victory };
    HeroStates heroState = HeroStates.Entering;

    public enum HeroDirections { Up, Down, Left, Right };
    HeroDirections heroDir;

    Vector2 curSpace;
    Vector2 nextSpace;

    List<string> inventory = new List<string>();

    public GameObject visibleLadderPrefab;
    GameObject visibleLadder;



    public override void Init(Tile[,] mapGrid, Vector2 initSpace)
    {
        this.mapGrid = mapGrid;
        curSpace = initSpace;

        //Top player spawn
        if (curSpace.y == (mapGrid.GetLength(1) - 1))
        {
            heroDir = HeroDirections.Down;
            nextSpace = new Vector2(curSpace.x, curSpace.y - 1);
        }

        //Bottom player spawn
        else if (curSpace.y == 0)
        {
            heroDir = HeroDirections.Up;
            nextSpace = new Vector2(curSpace.x, curSpace.y + 1);
        }

        //Left player spawn
        else if (curSpace.x == 0)
        {
            heroDir = HeroDirections.Right;
            nextSpace = new Vector2(curSpace.x + 1, curSpace.y);
        }

        //Right player spawn
        else if (curSpace.x == (mapGrid.GetLength(0) - 1))
        {
            heroDir = HeroDirections.Left;
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

                //When the player enters the mapGrid, set them to HeroStates.Moving
                if (nextSpace.x >= 0 && nextSpace.y >= 0 && nextSpace.x < mapGrid.GetLength(0) && nextSpace.y < mapGrid.GetLength(1))
                {
                    heroState = HeroStates.Moving;
                }
                break;

            case HeroStates.Moving:
                curSpace.x = nextSpace.x;
                curSpace.y = nextSpace.y;

                HandleCurrentSpace((int)curSpace.x, (int)curSpace.y);
                SetNextSpace((int)curSpace.x, (int)curSpace.y);
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
                SetNextSpace((int)curSpace.x, (int)curSpace.y);
                break;

            case HeroStates.SawPot:
                break;
            
            //Exactly like HeroStates.Entering, but without checking to see when the hero enters the map
            case HeroStates.Victory:
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
    void HandleCurrentSpace(int curX, int curY)
    {
        if (mapGrid[curX, curY] != null)
        {
            Tile checkTile = mapGrid[curX, curY].GetComponent<Tile>();

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
                    Destroy(mapGrid[curX, curY].gameObject);
                    break;

                case Tile.TileType.Sword:
                    if (!inventory.Contains("Sword")) inventory.Add("Sword");
                    Destroy(mapGrid[curX, curY].gameObject);
                    break;
            }

            //Cheeky check to see if we should delete a visible ladder
            if (visibleLadder != null)
            {
                if (checkTile.tileType != Tile.TileType.Pit) Destroy(visibleLadder);
            }
        }
        else
        {
            //Delete the visibleLadder if we're over a blank space
            if (visibleLadder != null) Destroy(visibleLadder);
        }
    }



    //Look ahead to see if there's something the hero has to deal with (e.g. walking out of bounds, running into an impassable tile, etc)
    void SetNextSpace(int curX, int curY)
    {
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

        if (nextX < 0 || nextX >= mapGrid.GetLength(0) || nextY < 0 || nextY >= mapGrid.GetLength(1))
        {
            //print("Out of bounds!!");
            heroState = HeroStates.Frustrated;
        }

        //If the space we're looking at isn't null, figure out what it is and what we should do about it
        else if (mapGrid[nextX, nextY] != null)
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
        switch (mapGrid[nextX, nextY].GetComponent<Tile>().tileType)
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

                    if (secondX < 0 || secondX >= mapGrid.GetLength(0) || secondY < 0 || secondY >= mapGrid.GetLength(1))
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
                        if (mapGrid[secondX, secondY] != null && mapGrid[secondX, secondY].GetComponent<Tile>().tileType == Tile.TileType.Pit)
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
                Destroy(this, 3);
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
        if (x < 0 || x >= mapGrid.GetLength(0) || y < 0 || y >= mapGrid.GetLength(1))
            return null;
        else
            return mapGrid[x, y].GetComponent<Tile>();
    }



    bool CheckForLineOfSightWithPot(int curX, int curY)
    {
        switch (heroDir)
        {
            case HeroDirections.Up:
                for (int i = curY; i < (mapGrid.GetLength(1) - 1); i++)
                {
                    Tile checkTile = mapGrid[curX, i];
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
                    Tile checkTile = mapGrid[curX, i];
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
                    Tile checkTile = mapGrid[i, curY];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Right:
                for (int i = curX; i < (mapGrid.GetLength(1) - 1); i++)
                {
                    Tile checkTile = mapGrid[i, curY];
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