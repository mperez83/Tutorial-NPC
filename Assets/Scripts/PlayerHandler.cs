using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [HideInInspector]
    public GameObject[,] mapGrid;

    public enum PlayerStates { Entering, Moving, Frustrated, SawPot, Victory };
    PlayerStates playerState = PlayerStates.Entering;

    public enum PlayerDirections { Up, Down, Left, Right };
    [HideInInspector]
    public PlayerDirections playerDir;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Vector2 playerSpace;
    [HideInInspector]
    public Vector2 nextSpace;
    public float timeToNextSpace;
    float curLerpTime = 0;

    List<string> inventory = new List<string>();



    void Update()
    {
        float actualLerpValue;  //Declared up here so we don't deal with "already declared" bullshit inside the switch statement

        switch (playerState)
        {
            //Exact same as PlayerStates.StaticMoving, except we set the player to PlayerStates.Moving when they enter the map
            case PlayerStates.Entering:
                curLerpTime += Time.deltaTime;
                if (curLerpTime >= timeToNextSpace) curLerpTime = timeToNextSpace;

                actualLerpValue = curLerpTime / timeToNextSpace;
                player.transform.position = Vector2.Lerp(playerSpace, nextSpace, actualLerpValue);

                if (curLerpTime == timeToNextSpace)
                {
                    curLerpTime = 0;
                    playerSpace.x = nextSpace.x;
                    playerSpace.y = nextSpace.y;

                    switch (playerDir)
                    {
                        case PlayerDirections.Up:
                            nextSpace.y = nextSpace.y + 1;
                            break;
                        case PlayerDirections.Down:
                            nextSpace.y = nextSpace.y - 1;
                            break;
                        case PlayerDirections.Left:
                            nextSpace.x = nextSpace.x - 1;
                            break;
                        case PlayerDirections.Right:
                            nextSpace.x = nextSpace.x + 1;
                            break;
                    }

                    //When the player enters the mapGrid, set them to PlayerStates.Moving
                    if (nextSpace.x >= 0 && nextSpace.y >= 0 && nextSpace.x < mapGrid.GetLength(0) && nextSpace.y < mapGrid.GetLength(1))
                    {
                        playerState = PlayerStates.Moving;
                    }
                }
                break;

            //For when the player is moving; this is the state the player will spend the most amount of time in
            case PlayerStates.Moving:
                curLerpTime += Time.deltaTime;
                if (curLerpTime >= timeToNextSpace) curLerpTime = timeToNextSpace;

                actualLerpValue = curLerpTime / timeToNextSpace;
                player.transform.position = Vector2.Lerp(playerSpace, nextSpace, actualLerpValue);

                if (curLerpTime == timeToNextSpace)
                {
                    curLerpTime = 0;
                    playerSpace.x = nextSpace.x;
                    playerSpace.y = nextSpace.y;

                    HandleCurrentSpace((int)playerSpace.x, (int)playerSpace.y);
                    SetNextSpace((int)playerSpace.x, (int)playerSpace.y);
                }
                break;

            //Causes the player to pause for a bit before turning around
            case PlayerStates.Frustrated:
                curLerpTime += Time.deltaTime;
                if (curLerpTime >= timeToNextSpace)
                {
                    curLerpTime = 0;
                    switch (playerDir)
                    {
                        case PlayerDirections.Up:
                            playerDir = PlayerDirections.Down;
                            break;
                        case PlayerDirections.Down:
                            playerDir = PlayerDirections.Up;
                            break;
                        case PlayerDirections.Left:
                            playerDir = PlayerDirections.Right;
                            break;
                        case PlayerDirections.Right:
                            playerDir = PlayerDirections.Left;
                            break;
                    }
                    playerState = PlayerStates.Moving;
                    SetNextSpace((int)playerSpace.x, (int)playerSpace.y);
                }
                break;

            //Not sure if this is needed? The player enters this state when they see the pot, but PlayerHandler is deleted anyway once they do see it
            case PlayerStates.SawPot:
                //Not sure if anything should go here
                break;

            //Like PlayerStates.Moving, except we don't use the SetNextSpace function
            case PlayerStates.Victory:
                curLerpTime += Time.deltaTime;
                if (curLerpTime >= timeToNextSpace) curLerpTime = timeToNextSpace;

                actualLerpValue = curLerpTime / timeToNextSpace;
                player.transform.position = Vector2.Lerp(playerSpace, nextSpace, actualLerpValue);

                if (curLerpTime == timeToNextSpace)
                {
                    curLerpTime = 0;
                    playerSpace.x = nextSpace.x;
                    playerSpace.y = nextSpace.y;

                    switch (playerDir)
                    {
                        case PlayerDirections.Up:
                            nextSpace.y = nextSpace.y + 1;
                            break;
                        case PlayerDirections.Down:
                            nextSpace.y = nextSpace.y - 1;
                            break;
                        case PlayerDirections.Left:
                            nextSpace.x = nextSpace.x - 1;
                            break;
                        case PlayerDirections.Right:
                            nextSpace.x = nextSpace.x + 1;
                            break;
                    }
                }
                break;
        }

    }





    //If something is at the hero's space, decide what to do (e.g. change direction, pick up item, etc)
    void HandleCurrentSpace(int curX, int curY)
    {
        if (mapGrid[curX, curY] != null)
        {
            switch (mapGrid[curX, curY].GetComponent<Tile>().tileType)
            {
                case Tile.TileType.TurnUp:
                    playerDir = PlayerDirections.Up;
                    break;

                case Tile.TileType.TurnDown:
                    playerDir = PlayerDirections.Down;
                    break;

                case Tile.TileType.TurnLeft:
                    playerDir = PlayerDirections.Left;
                    break;

                case Tile.TileType.TurnRight:
                    playerDir = PlayerDirections.Right;
                    break;

                case Tile.TileType.Ladder:
                    if (!inventory.Contains("Ladder")) inventory.Add("Ladder");
                    Destroy(mapGrid[curX, curY]);
                    break;
            }
        }
    }



    //Look ahead to see if there's something the hero has to deal with (e.g. walking out of bounds, running into an impassable tile, etc)
    void SetNextSpace(int curX, int curY)
    {
        if (CheckForLineOfSightWithPot(curX, curY))
        {
            print("THE HERO SAW THE POT");
            playerState = PlayerStates.SawPot;
            Destroy(this);
            return;
        }

        int nextX = curX;
        int nextY = curY;

        switch (playerDir)
        {
            case PlayerDirections.Up:
                nextY = curY + 1;
                break;

            case PlayerDirections.Down:
                nextY = curY - 1;
                break;

            case PlayerDirections.Left:
                nextX = curX - 1;
                break;

            case PlayerDirections.Right:
                nextX = curX + 1;
                break;
        }

        if (nextX < 0 || nextX >= mapGrid.GetLength(0) || nextY < 0 || nextY >= mapGrid.GetLength(1))
        {
            print("Out of bounds!!");
            playerState = PlayerStates.Frustrated;
        }

        //If the space we're looking at isn't null, figure out what it is and what we should do about it
        else if (mapGrid[nextX, nextY] != null)
        {
            switch (mapGrid[nextX, nextY].GetComponent<Tile>().tileType)
            {
                case Tile.TileType.Pit:
                    if (!inventory.Contains("Ladder"))
                    {
                        print("Can't pass!!!");
                        playerState = PlayerStates.Frustrated;
                    }
                    else
                    {
                        int secondX = nextX;
                        int secondY = nextY;
                        switch (playerDir)
                        {
                            case PlayerDirections.Up:
                                secondY = nextY + 1;
                                break;

                            case PlayerDirections.Down:
                                secondY = nextY - 1;
                                break;

                            case PlayerDirections.Left:
                                secondX = nextX - 1;
                                break;

                            case PlayerDirections.Right:
                                secondX = nextX + 1;
                                break;
                        }

                        if (secondX < 0 || secondX >= mapGrid.GetLength(0) || secondY < 0 || secondY >= mapGrid.GetLength(1))
                        {
                            print("Out of bounds, so it's ok if the player crosses with a ladder because they'll bump into a wall");
                        }
                        else
                        {
                            if (mapGrid[secondX, secondY] != null && mapGrid[secondX, secondY].GetComponent<Tile>().tileType == Tile.TileType.Pit)
                            {
                                print("The gap is too wide to cross!!!");
                                playerState = PlayerStates.Frustrated;
                            }
                            else
                            {
                                print("You used your ladder to cross the gap!!");
                                nextSpace.x = nextX;
                                nextSpace.y = nextY;
                            }
                        }
                    }
                    break;

                case Tile.TileType.Exit:
                    print("You won!!!");
                    playerState = PlayerStates.Victory;
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

        //If the space we're looking at is null, just move forward
        else
        {
            //print("All clear to keep moving!");
            nextSpace.x = nextX;
            nextSpace.y = nextY;
        }
    }

    Tile GetTileAtCoords(int x, int y)
    {
        if (x < 0 || x >= mapGrid.GetLength(0) || y < 0 || y >= mapGrid.GetLength(1))
        {
            return null;
        }
        else
        {
            return mapGrid[x, y].GetComponent<Tile>();
        }
    }



    bool CheckForLineOfSightWithPot(int curX, int curY)
    {
        switch (playerDir)
        {
            case PlayerDirections.Up:
                for (int i = curY; i < (mapGrid.GetLength(1) - 1); i++)
                {
                    GameObject checkTile = mapGrid[curX, i];
                    if (checkTile != null)
                        if (checkTile.GetComponent<Tile>().tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.GetComponent<Tile>().blocksVision)
                            return false;
                }
                break;
            case PlayerDirections.Down:
                for (int i = curY; i > 0; i--)
                {
                    GameObject checkTile = mapGrid[curX, i];
                    if (checkTile != null)
                        if (checkTile.GetComponent<Tile>().tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.GetComponent<Tile>().blocksVision)
                            return false;
                }
                break;
            case PlayerDirections.Left:
                for (int i = curX; i > 0; i--)
                {
                    GameObject checkTile = mapGrid[i, curY];
                    if (checkTile != null)
                        if (checkTile.GetComponent<Tile>().tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.GetComponent<Tile>().blocksVision)
                            return false;
                }
                break;
            case PlayerDirections.Right:
                for (int i = curX; i < (mapGrid.GetLength(1) - 1); i++)
                {
                    GameObject checkTile = mapGrid[i, curY];
                    if (checkTile != null)
                        if (checkTile.GetComponent<Tile>().tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.GetComponent<Tile>().blocksVision)
                            return false;
                }
                break;
        }

        return false;
    }
}