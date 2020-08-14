using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public readonly Vector2Int leftUp;
    public readonly Vector2Int rightDown;
    public int Left => leftUp.x;
    public int Right => rightDown.x;
    public int Up => leftUp.y;
    public int Down => rightDown.y;

    public Room(Vector2Int leftUpBound, Vector2Int rightDownBound)
    {
        (this.leftUp, this.rightDown) = (leftUpBound, rightDownBound);
    }

    public void Apply(GeneratedMap.MapInTheMaking dst)
    {
        for(int i = this.leftUp.x; i < this.rightDown.x + 1; i ++)
        {
            for(int j = this.leftUp.y; j < this.rightDown.y + 1; j ++)
            {
                dst.tiles[j][i] = TileType.Floor;
            }
        }
    }
}
