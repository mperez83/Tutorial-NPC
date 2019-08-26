using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public GameObject[,] mapGrid;

    //Player stuff
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Vector2 playerSpace;

    public enum PlayerState { Moving, Frustrated };
    PlayerState playerState = PlayerState.Moving;

    public enum PlayerDirections { Up, Down, Left, Right };
    PlayerDirections playerDir = PlayerDirections.Up;

    [HideInInspector]
    public Vector2 nextSpace;
    float curLerpTime = 0;
    public float timeToNextSpace;



    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Moving:
                curLerpTime += Time.deltaTime;
                if (curLerpTime >= timeToNextSpace) curLerpTime = timeToNextSpace;

                float actualLerpValue = curLerpTime / timeToNextSpace;
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

            case PlayerState.Frustrated:
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
                    playerState = PlayerState.Moving;
                    SetNextSpace((int)playerSpace.x, (int)playerSpace.y);
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

        if (nextX < 0 || nextX >= mapGrid.GetLength(0) || nextY < 0 || nextY >= mapGrid.GetLength(1))
        {
            print("Out of bounds!!");
            playerState = PlayerState.Frustrated;
        }
        else if (mapGrid[nextX, nextY] != null && mapGrid[nextX, nextY].GetComponent<Tile>().tileType == Tile.TileType.Impassable)
        {
            print("Can't pass!!!");
            playerState = PlayerState.Frustrated;
        }
        else
        {
            print("All clear to keep moving!");
            nextSpace.x = nextX;
            nextSpace.y = nextY;
        }
    }
}