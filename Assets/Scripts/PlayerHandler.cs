﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [HideInInspector]
    public MapHandler mapHandler;

    //Player stuff
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Vector2 playerSpace;

    public enum PlayerStates { Entering, Moving, Frustrated, Victory };
    PlayerStates playerState = PlayerStates.Entering;

    public enum PlayerDirections { Up, Down, Left, Right };
    [HideInInspector]
    public PlayerDirections playerDir;

    [HideInInspector]
    public Vector2 nextSpace;
    float curLerpTime = 0;
    public float timeToNextSpace;



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
                    GameObject[,] mapGrid = mapHandler.GetMapGrid();
                    if (nextSpace.x >= 0 && nextSpace.y >= 0 && nextSpace.x < mapGrid.GetLength(0) && nextSpace.y < mapGrid.GetLength(1))
                    {
                        playerState = PlayerStates.Moving;
                    }
                }
                break;
            
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
        GameObject[,] mapGrid = mapHandler.GetMapGrid();

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
            }
        }
    }



    //Look ahead to see if there's something the hero has to deal with (e.g. walking out of bounds, running into an impassable tile, etc)
    void SetNextSpace(int curX, int curY)
    {
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

        GameObject[,] mapGrid = mapHandler.GetMapGrid();

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
                case Tile.TileType.Impassable:
                    print("Can't pass!!!");
                    playerState = PlayerStates.Frustrated;
                    break;

                case Tile.TileType.Exit:
                    print("You won!!!");
                    playerState = PlayerStates.Victory;
                    nextSpace.x = nextX;
                    nextSpace.y = nextY;
                    Destroy(this, 3);
                    break;

                default:
                    print("All clear to keep moving!");
                    nextSpace.x = nextX;
                    nextSpace.y = nextY;
                    break;
            }
        }

        //If the space we're looking at is null, just move forward
        else
        {
            print("All clear to keep moving!");
            nextSpace.x = nextX;
            nextSpace.y = nextY;
        }
    }
}