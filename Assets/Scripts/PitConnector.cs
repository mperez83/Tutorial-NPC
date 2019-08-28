using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitConnector : MonoBehaviour
{
    //Solo pit
    public Sprite solo;

    //Main stuff
    public Sprite topLeft;
    public Sprite topCenter;
    public Sprite topRight;
    public Sprite centerLeft;
    public Sprite center;
    public Sprite centerRight;
    public Sprite bottomLeft;
    public Sprite bottomCenter;
    public Sprite bottomRight;

    //Channel stuff
    public Sprite channelEndLeft;
    public Sprite channelStraightHorizontal;
    public Sprite channelEndRight;

    public Sprite channelEndBottom;
    public Sprite channelStraightVertical;
    public Sprite channelEndTop;

    public Sprite channelCornerTopLeft;
    public Sprite channelCornerTopRight;
    public Sprite channelCornerBottomLeft;
    public Sprite channelCornerBottomRight;

    public Sprite tChannelUp;
    public Sprite tChannelDown;
    public Sprite tChannelLeft;
    public Sprite tChannelRight;

    //Corner stuff
    public Sprite allFourCorners;

    public Sprite threeCorners124;
    public Sprite threeCorners134;
    public Sprite threeCorners234;
    public Sprite threeCorners123;

    public Sprite oppositeCornersTLBR;
    public Sprite oppositeCornersBLTR;

    public Sprite onlyTopCorners;
    public Sprite onlyBottomCorners;
    public Sprite onlyLeftCorners;
    public Sprite onlyRightCorners;

    public Sprite onlyTopLeftCorner;
    public Sprite onlyTopRightCorner;
    public Sprite onlyBottomLeftCorner;
    public Sprite onlyBottomRightCorner;

    //Special stuff
    public Sprite specialBottomRight;
    public Sprite specialBottomLeft;
    public Sprite specialTopRight;
    public Sprite specialTopLeft;

    public Sprite verticalSpecialBottomRight;
    public Sprite verticalSpecialBottomLeft;
    public Sprite verticalSpecialTopRight;
    public Sprite verticalSpecialTopLeft;

    public void ConnectAllPits(Tile[,] mapGrid)
    {
        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (int j = 0; j < mapGrid.GetLength(1); j++)
            {
                if (mapGrid[i, j] != null && mapGrid[i, j].tileType == Tile.TileType.Pit)
                {
                    string finalStr = null;

                    //Start from bottom left, ascend to top right
                    finalStr += (GetIfPitAtCoords(mapGrid, i - 1, j - 1)) ? "1" : "0";
                    finalStr += (GetIfPitAtCoords(mapGrid, i, j - 1)) ? "1" : "0";
                    finalStr += (GetIfPitAtCoords(mapGrid, i + 1, j - 1)) ? "1" : "0";

                    finalStr += (GetIfPitAtCoords(mapGrid, i - 1, j)) ? "1" : "0";
                    finalStr += "1";
                    finalStr += (GetIfPitAtCoords(mapGrid, i + 1, j)) ? "1" : "0";

                    finalStr += (GetIfPitAtCoords(mapGrid, i - 1, j + 1)) ? "1" : "0";
                    finalStr += (GetIfPitAtCoords(mapGrid, i, j + 1)) ? "1" : "0";
                    finalStr += (GetIfPitAtCoords(mapGrid, i + 1, j + 1)) ? "1" : "0";

                    switch (finalStr)
                    {
                        //Top left corner
                        case "011011000":
                        case "011011001":
                        case "111011000":
                        case "011011101":
                        case "111011001":
                        case "111011100":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = topLeft;
                            break;
                        
                        //Top center
                        case "111111000":
                        case "111111001":
                        case "111111100":
                        case "111111101":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = topCenter;
                            break;
                        
                        //Top right corner
                        case "110110000":
                        case "111110000":
                        case "111110100":
                        case "110110100":
                        case "111110001":
                        case "110110001":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = topRight;
                            break;

                        //Middle left
                        case "011011011":
                        case "111011111":
                        case "111011011":
                        case "011011111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = centerLeft;
                            break;

                        //Middle
                        case "111111111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = center;
                            break;

                        //Middle right
                        case "110110110":
                        case "111110111":
                        case "111110110":
                        case "110110111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = centerRight;
                            break;

                        //Bottom left corner
                        case "000011011":
                        case "001011011":
                        case "001011111":
                        case "000011111":
                        case "100011011":
                        case "101011111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = bottomLeft;
                            break;

                        //Bottom center
                        case "000111111":
                        case "001111111":
                        case "100111111":
                        case "101111111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = bottomCenter;
                            break;

                        //Bottom right corner
                        case "000110110":
                        case "101110111":
                        case "000110111":
                        case "100110110":
                        case "100110111":
                        case "101110110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = bottomRight;
                            break;

                        //Channel end left
                        case "000011000":
                        case "100011000":
                        case "001011100":
                        case "001011001":
                        case "000011100":
                        case "001011000":
                        case "000011001":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelEndLeft;
                            break;

                        //Channel straight horizontal
                        case "000111000":
                        case "101111000":
                        case "000111101":
                        case "000111100":
                        case "000111001":
                        case "100111000":
                        case "001111000":
                        case "001111001":
                        case "100111100":
                        case "001111101":
                        case "001111100":
                        case "100111001":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelStraightHorizontal;
                            break;

                        //Channel end right
                        case "000110000":
                        case "100110001":
                        case "000110001":
                        case "100110100":
                        case "001110000":
                        case "100110000":
                        case "001110100":
                        case "000110100":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelEndRight;
                            break;

                        //Channel end bottom
                        case "000010010":
                        case "100010011":
                        case "001010011":
                        case "000010110":
                        case "100010110":
                        case "001010010":
                        case "100010010":
                        case "000010111":
                        case "100010111":
                        case "001010111":
                        case "101010111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelEndBottom;
                            break;

                        //Channel straight vertical
                        case "010010010":
                        case "011010011":
                        case "110010110":
                        case "011010010":
                        case "110010010":
                        case "010010011":
                        case "010010110":
                        case "111010110":
                        case "111010011":
                        case "110010011":
                        case "011010110":
                        case "111010010":
                        case "111010111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelStraightVertical;
                            break;

                        //Channel end top
                        case "010010000":
                        case "011010000":
                        case "110010100":
                        case "011010100":
                        case "110010101":
                        case "110010000":
                        case "010010100":
                        case "110010001":
                        case "010010001":
                        case "111010000":
                        case "111010100":
                        case "111010001":
                        case "111010101":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelEndTop;
                            break;

                        //Channel corner top left
                        case "010011000":
                        case "110011101":
                        case "110011100":
                        case "110011001":
                        case "010011001":
                        case "110011000":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelCornerTopLeft;
                            break;

                        //Channel corner top right
                        case "010110000":
                        case "011110100":
                        case "010110100":
                        case "011110000":
                        case "011110101":
                        case "011110001":
                        case "010110001":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelCornerTopRight;
                            break;

                        //Channel corner bottom left
                        case "000011010":
                        case "001011010":
                        case "001011110":
                        case "000011110":
                        case "100011110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelCornerBottomLeft;
                            break;

                        //Channel corner bottom right
                        case "000110010":
                        case "000110011":
                        case "100110010":
                        case "101110011":
                        case "100110011":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = channelCornerBottomRight;
                            break;

                        //T channel up
                        case "000111010":
                        case "100111010":
                        case "001111010":
                        case "101111010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = tChannelUp;
                            break;

                        //T channel down
                        case "010111000":
                        case "010111100":
                        case "010111001":
                        case "010111101":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = tChannelDown;
                            break;

                        //T channel left
                        case "011110011":
                        case "010110011":
                        case "010110010":
                        case "011110010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = tChannelLeft;
                            break;

                        //T channel right
                        case "010011010":
                        case "110011010":
                        case "110011110":
                        case "010011110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = tChannelRight;
                            break;

                        //All four corners
                        case "010111010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = allFourCorners;
                            break;

                        //Three corners 124
                        case "110111010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = threeCorners124;
                            break;

                        //Three corners 134
                        case "010111110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = threeCorners134;
                            break;

                        //Three corners 234
                        case "010111011":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = threeCorners234;
                            break;

                        //Three corners 123
                        case "011111010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = threeCorners123;
                            break;

                        //Opposite corners TL BR
                        case "110111011":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = oppositeCornersTLBR;
                            break;

                        //Opposite corners BL TR
                        case "011111110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = oppositeCornersBLTR;
                            break;

                        //Only top corners
                        case "111111010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyTopCorners;
                            break;

                        //Only bottom corners
                        case "010111111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyBottomCorners;
                            break;

                        //Only left corners
                        case "011111011":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyLeftCorners;
                            break;
                        
                        //Only right corners
                        case "110111110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyRightCorners;
                            break;

                        //Only top left corner
                        case "111111011":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyTopLeftCorner;
                            break;
                        
                        //Only top right corner
                        case "111111110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyTopRightCorner;
                            break;
                        
                        //Only bottom left corner
                        case "011111111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyBottomLeftCorner;
                            break;
                        
                        //Only bottom right corner
                        case "110111111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = onlyBottomRightCorner;
                            break;

                        //Special bottom right
                        case "110111001":
                        case "110111000":
                        case "110111100":
                        case "110111101":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = specialBottomRight;
                            break;

                        //Special bottom left
                        case "011111000":
                        case "011111100":
                        case "011111001":
                        case "011111101":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = specialBottomLeft;
                            break;

                        //Special top right
                        case "100111110":
                        case "101111110":
                        case "001111110":
                        case "000111110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = specialTopRight;
                            break;

                        //Special top left
                        case "000111011":
                        case "100111011":
                        case "001111011":
                        case "101111011":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = specialTopLeft;
                            break;

                        //Vertical special bottom right
                        case "110011011":
                        case "010011011":
                        case "110011111":
                        case "010011111":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = verticalSpecialBottomRight;
                            break;

                        //Vertical special bottom left
                        case "010110111":
                        case "011110111":
                        case "011110110":
                        case "010110110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = verticalSpecialBottomLeft;
                            break;

                        //Vertical special top right
                        case "011011010":
                        case "111011010":
                        case "011011110":
                        case "111011110":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = verticalSpecialTopRight;
                            break;

                        //Vertical special top left
                        case "110110011":
                        case "110110010":
                        case "111110011":
                        case "111110010":
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = verticalSpecialTopLeft;
                            break;

                        //Default is a solo pit
                        default:
                            mapGrid[i, j].GetComponent<SpriteRenderer>().sprite = solo;
                            break;

                    }
                }
            }
        }
    }

    bool GetIfPitAtCoords(Tile[,] mapGrid, int x, int y)
    {
        if (x >= 0 && y >= 0 && x < mapGrid.GetLength(0) && y < mapGrid.GetLength(1))
        {
            if (mapGrid[x, y] != null)
                return (mapGrid[x, y].tileType == Tile.TileType.Pit);
            else
                return false;
        }
        else
        {
            return false;
        }
    }
}