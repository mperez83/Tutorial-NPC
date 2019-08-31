using System.Collections.Generic;
using UnityEngine;

public class HeroHandler : MapEntity
{
    MapHandlerExp mapHandler;

    public enum HeroStates { Entering, Moving, Frustrated, SawPot, Victory };
    HeroStates heroState = HeroStates.Entering;

    public enum HeroDirections { Up, Down, Left, Right };
    HeroDirections heroDir;

    Vector2 curSpace;
    Vector2 nextSpace;

    List<string> inventory = new List<string>();

    public GameObject visibleLadderPrefab;
    GameObject visibleLadder;

    public GameOverUI gameOverUI;
    public VictoryUI victoryUI;

    public Sprite entryArrow;
    public Sprite[] heroSprites;



    public override void Init(MapHandlerExp mapHandler, Vector2 initSpace)
    {
        this.mapHandler = mapHandler;
        GetComponent<SpriteRenderer>().sprite = entryArrow;

        curSpace = initSpace;

        //Top player spawn
        if (curSpace.y == (mapHandler.tileGrid.GetLength(1) - 1))
        {
            heroDir = HeroDirections.Down;
            transform.localEulerAngles = new Vector3(0, 0, 180);
            curSpace = new Vector2(curSpace.x, curSpace.y + 3);
            nextSpace = new Vector2(curSpace.x, curSpace.y - 1);
        }

        //Bottom player spawn
        else if (curSpace.y == 0)
        {
            heroDir = HeroDirections.Up;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            curSpace = new Vector2(curSpace.x, curSpace.y - 3);
            nextSpace = new Vector2(curSpace.x, curSpace.y + 1);
        }

        //Left player spawn
        else if (curSpace.x == 0)
        {
            heroDir = HeroDirections.Right;
            transform.localEulerAngles = new Vector3(0, 0, 270);
            curSpace = new Vector2(curSpace.x - 3, curSpace.y);
            nextSpace = new Vector2(curSpace.x + 1, curSpace.y);
        }

        //Right player spawn
        else if (curSpace.x == (mapHandler.tileGrid.GetLength(0) - 1))
        {
            heroDir = HeroDirections.Left;
            transform.localEulerAngles = new Vector3(0, 0, 90);
            curSpace = new Vector2(curSpace.x + 3, curSpace.y);
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



    public override void OnMapActivate()
    {
        UpdateHeroGraphic();
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }



    public override void OnMapUpdate(float actionTimer, float actionTimerLength)
    {
        float actualLerpValue;  //Declared up here so we don't deal with "already declared" bullshit inside the switch statement

        switch (heroState)
        {
            case HeroStates.Entering:
                actualLerpValue = (actionTimerLength - actionTimer) / actionTimerLength;
                transform.position = Vector2.Lerp(curSpace, nextSpace, actualLerpValue);
                break;

            case HeroStates.Moving:
                actualLerpValue = (actionTimerLength - actionTimer) / actionTimerLength;
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
                actualLerpValue = (actionTimerLength - actionTimer) / actionTimerLength;
                transform.position = Vector2.Lerp(curSpace, nextSpace, actualLerpValue);
                break;
        }

    }



    public override void OnMapAction()
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

                //When the player enters the mapHandler.tileGrid, set them to HeroStates.Moving
                if (nextSpace.x >= 0 && nextSpace.y >= 0 && nextSpace.x < mapHandler.tileGrid.GetLength(0) && nextSpace.y < mapHandler.tileGrid.GetLength(1))
                {
                    heroState = HeroStates.Moving;
                }
                break;

            case HeroStates.Moving:
                curSpace.x = nextSpace.x;
                curSpace.y = nextSpace.y;

                //If the hero is over a tile, do something
                if (mapHandler.tileGrid[(int)curSpace.x, (int)curSpace.y]) HandleCurrentTile();
                else
                {
                    if (visibleLadder != null) Destroy(visibleLadder);  //Really bad, kinda hope we can move this elsewhere at some point
                }

                //If the hero is over an entity, do something
                if (mapHandler.entityGrid[(int)curSpace.x, (int)curSpace.y]) HandleCurrentEntity();

                SetNextSpace();
                break;

            case HeroStates.Frustrated:
                switch (heroDir)
                {
                    case HeroDirections.Up:
                        ChangeHeroDirection(HeroDirections.Down);
                        break;
                    case HeroDirections.Down:
                        ChangeHeroDirection(HeroDirections.Up);
                        break;
                    case HeroDirections.Left:
                        ChangeHeroDirection(HeroDirections.Right);
                        break;
                    case HeroDirections.Right:
                        ChangeHeroDirection(HeroDirections.Left);
                        break;
                }
                heroState = HeroStates.Moving;
                SetNextSpace();
                break;

            case HeroStates.SawPot:
                break;

            case HeroStates.Victory:
                if (!GameMaster.instance.GetIfCoordsAreInsideCam(curSpace.x, curSpace.y))
                {
                    victoryUI.ActivateVictoryScreen();
                }
                else
                {
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
                }
                break;
        }
    }





    //If something is at the hero's space, decide what to do (e.g. change direction, pick up item, etc)
    void HandleCurrentTile()
    {
        int curX = (int)curSpace.x;
        int curY = (int)curSpace.y;

        Tile checkTile = mapHandler.tileGrid[curX, curY].GetComponent<Tile>();

        switch (checkTile.tileType)
        {
            case Tile.TileType.TurnUp:
                ChangeHeroDirection(HeroDirections.Up);
                break;

            case Tile.TileType.TurnDown:
                ChangeHeroDirection(HeroDirections.Down);
                break;

            case Tile.TileType.TurnLeft:
                ChangeHeroDirection(HeroDirections.Left);
                break;

            case Tile.TileType.TurnRight:
                ChangeHeroDirection(HeroDirections.Right);
                break;

            case Tile.TileType.Pit:
                if (!inventory.Contains("Ladder"))
                {
                    gameOverUI.ActivateGameOver("Game over!\n(The hero fell into a pit)");
                }
                else
                {
                    int x = (int)curSpace.x;
                    int y = (int)curSpace.y;
                    Tile extraPitCheck;

                    switch (heroDir)
                    {
                        case HeroDirections.Up:
                            extraPitCheck = mapHandler.GetTileAtCoords(x, y + 1);
                            if (extraPitCheck && extraPitCheck.tileType == Tile.TileType.Pit) gameOverUI.ActivateGameOver("Game over!\n(The pit was too wide to cross with a ladder)");
                            break;

                        case HeroDirections.Down:
                            extraPitCheck = mapHandler.GetTileAtCoords(x, y - 1);
                            if (extraPitCheck && extraPitCheck.tileType == Tile.TileType.Pit) gameOverUI.ActivateGameOver("Game over!\n(The pit was too wide to cross with a ladder)");
                            break;

                        case HeroDirections.Left:
                            extraPitCheck = mapHandler.GetTileAtCoords(x - 1, y);
                            if (extraPitCheck && extraPitCheck.tileType == Tile.TileType.Pit) gameOverUI.ActivateGameOver("Game over!\n(The pit was too wide to cross with a ladder)");
                            break;

                        case HeroDirections.Right:
                            extraPitCheck = mapHandler.GetTileAtCoords(x + 1, y);
                            if (extraPitCheck && extraPitCheck.tileType == Tile.TileType.Pit) gameOverUI.ActivateGameOver("Game over!\n(The pit was too wide to cross with a ladder)");
                            break;
                    }
                }
                break;

            case Tile.TileType.Ladder:
                if (!inventory.Contains("Ladder")) inventory.Add("Ladder");
                Destroy(mapHandler.tileGrid[curX, curY].gameObject);
                break;

            case Tile.TileType.Sword:
                if (!inventory.Contains("Sword")) inventory.Add("Sword");
                Destroy(mapHandler.tileGrid[curX, curY].gameObject);
                break;

            case Tile.TileType.Exit:
                heroState = HeroStates.Victory;
                //Top player exit
                if (curY == (mapHandler.tileGrid.GetLength(1) - 1))
                    ChangeHeroDirection(HeroDirections.Up);
                //Bottom player exit
                else if (curY == 0)
                    ChangeHeroDirection(HeroDirections.Down);
                //Left player exit
                else if (curX == 0)
                    ChangeHeroDirection(HeroDirections.Left);
                //Right player exit
                else if (curX == (mapHandler.tileGrid.GetLength(0) - 1))
                    ChangeHeroDirection(HeroDirections.Right);
                else
                    print("The exit was in an illegal position (not along the edges of the map) so we're leaving the hero's direction alone");
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

        MapEntity checkEntity = mapHandler.entityGrid[curX, curY].GetComponent<MapEntity>();
        if (checkEntity.isEnemy)
        {
            if (inventory.Contains("Sword"))
            {
                Destroy(mapHandler.entityGrid[curX, curY].gameObject);
            }
            else
            {
                gameOverUI.ActivateGameOver("Game over!\n(The hero died)");
            }
        }
    }



    //Look ahead to see if there's something the hero has to deal with (e.g. walking out of bounds, running into an impassable tile, etc)
    void SetNextSpace()
    {
        //First, check if the hero saw the pot
        bool sawPot = CheckForLineOfSightWithPot();
        if (sawPot)
        {
            heroState = HeroStates.SawPot;
            gameOverUI.ActivateGameOver("Game over!\n(The hero saw a pot)");
            return;
        }



        int curX = (int)curSpace.x;
        int curY = (int)curSpace.y;

        int nextX = curX;
        int nextY = curY;

        //Update where the hero is going next
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

        if (!mapHandler.GetIfInsideTileGrid(nextX, nextY))
        {
            if (heroState == HeroStates.Victory)
            {
                //print("The hero is currently escaping, so it's ok to move out of bounds!");
                nextSpace.x = nextX;
                nextSpace.y = nextY;
            }
            else
            {
                //print("Out of bounds!!");
                heroState = HeroStates.Frustrated;
            }
        }

        //If the space we're looking at isn't null, figure out what it is and what we should do about it
        else if (mapHandler.tileGrid[nextX, nextY] != null)
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
        switch (mapHandler.tileGrid[nextX, nextY].GetComponent<Tile>().tileType)
        {
            case Tile.TileType.Pit:
                if (!inventory.Contains("Ladder"))
                {
                    //print("The hero is going to fall down a pit!!!");
                    nextSpace.x = nextX;
                    nextSpace.y = nextY;
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

                    if (secondX < 0 || secondX >= mapHandler.tileGrid.GetLength(0) || secondY < 0 || secondY >= mapHandler.tileGrid.GetLength(1))
                    {
                        //print("Out of bounds, so it's ok if the hero crosses with a ladder because they'll bump into a wall");
                        nextSpace.x = nextX;
                        nextSpace.y = nextY;

                        if (visibleLadder != null) Destroy(visibleLadder);
                        visibleLadder = Instantiate(visibleLadderPrefab, new Vector2(nextSpace.x, nextSpace.y), Quaternion.identity);
                        if (horizontal) visibleLadder.transform.Rotate(new Vector3(0, 0, 90));
                    }
                    else
                    {
                        if (mapHandler.tileGrid[secondX, secondY] != null && mapHandler.tileGrid[secondX, secondY].GetComponent<Tile>().tileType == Tile.TileType.Pit)
                        {
                            //print("The pit is too wide to cross with a ladder, so the hero is going to fall!!!");
                            nextSpace.x = nextX;
                            nextSpace.y = nextY;
                        }
                        else
                        {
                            //print("The hero used their ladder to cross the gap!!");
                            nextSpace.x = nextX;
                            nextSpace.y = nextY;

                            if (visibleLadder != null) Destroy(visibleLadder);
                            visibleLadder = Instantiate(visibleLadderPrefab, new Vector2(nextSpace.x, nextSpace.y), Quaternion.identity);
                            if (horizontal) visibleLadder.transform.Rotate(new Vector3(0, 0, 90));
                        }
                    }
                }
                break;

            case Tile.TileType.Rock:
                heroState = HeroStates.Frustrated;
                break;

            case Tile.TileType.Exit:
                //print("The hero is about to exit!!!");
                nextSpace.x = nextX;
                nextSpace.y = nextY;
                break;

            default:
                //print("This shouldn't ever print??");
                nextSpace.x = nextX;
                nextSpace.y = nextY;
                break;
        }
    }







    //Helper functions

    bool CheckForLineOfSightWithPot()
    {
        int x = (int)curSpace.x;
        int y = (int)curSpace.y;

        switch (heroDir)
        {
            case HeroDirections.Up:
                for (int i = y; i < (mapHandler.tileGrid.GetLength(1) - 1); i++)
                {
                    Tile checkTile = mapHandler.tileGrid[x, i];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Down:
                for (int i = y; i > 0; i--)
                {
                    Tile checkTile = mapHandler.tileGrid[x, i];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Left:
                for (int i = x; i > 0; i--)
                {
                    Tile checkTile = mapHandler.tileGrid[i, y];
                    if (checkTile != null)
                        if (checkTile.tileType == Tile.TileType.Pot)
                            return true;
                        else if (checkTile.blocksVision)
                            return false;
                }
                break;
            case HeroDirections.Right:
                for (int i = x; i < (mapHandler.tileGrid.GetLength(0) - 1); i++)
                {
                    Tile checkTile = mapHandler.tileGrid[i, y];
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



    void ChangeHeroDirection(HeroDirections newDir)
    {
        heroDir = newDir;
        UpdateHeroGraphic();
    }

    void UpdateHeroGraphic()
    {
        switch (heroDir)
        {
            case HeroDirections.Up:
                GetComponent<SpriteRenderer>().sprite = heroSprites[0];
                break;

            case HeroDirections.Down:
                GetComponent<SpriteRenderer>().sprite = heroSprites[1];
                break;

            case HeroDirections.Left:
                GetComponent<SpriteRenderer>().sprite = heroSprites[2];
                break;

            case HeroDirections.Right:
                GetComponent<SpriteRenderer>().sprite = heroSprites[3];
                break;
        }
    }
}