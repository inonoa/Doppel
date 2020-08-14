using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridUnit
{
    public readonly Vector2Int leftUp;
    public readonly Vector2Int rightDown;
    public int Left => leftUp.x;
    public int Right => rightDown.x;
    public int Up => leftUp.y;
    public int Down => rightDown.y;

    Room _Room;
    public Room Room => _Room;

    public GridUnit(Vector2Int leftUpBound, Vector2Int rightDownBound)
    {
        (this.leftUp, this.rightDown) = (leftUpBound, rightDownBound);
    }

    //一辺を完全に共有しているときのみtrue
    public bool IsNextTo(GridUnit other)
    {
        bool leftIsNext  = this.Left    == other.Right + 1;
        bool rightIsNext = this.Right   == other.Left    - 1;
        if(leftIsNext || rightIsNext)
        {
            return this.Up    == other.Up
                && this.Down  == other.Down;
        }

        bool upIsNext   = this.Up    == other.Down + 1;
        bool downIsNext = this.Down == other.Up    - 1;
        if(upIsNext || downIsNext)
        {
            return this.Left    == other.Left
                && this.Right == other.Right;
        }

        return false;
    }

    public GridUnit Combine(GridUnit other)
    {
        if(this.Left < other.Left
        || this.Up < other.Up)
        {
            return new GridUnit(this.leftUp, other.rightDown);
        }
        else
        {
            return new GridUnit(other.leftUp, this.rightDown);
        }
    }

    public Room CreateRoom()
    {
        Vector2Int lu = new Vector2Int();
        lu.x = Random.Range(this.Left + 1, this.Right - 3);
        lu.y = Random.Range(this.Up + 1, this.Down - 3);
            
        Vector2Int rd = new Vector2Int();
        rd.x = Random.Range(lu.x + 2, this.Right - 1);
        rd.y = Random.Range(lu.y + 2, this.Down - 1);

        return _Room = new Room(lu, rd);
    }
}
