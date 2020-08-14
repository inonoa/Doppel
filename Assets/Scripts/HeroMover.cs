using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMover : MonoBehaviour
{
    public enum Direction{ L, R, U, D }

    public Vector2Int PosOnMap{ get; private set; }

    public void Init(Vector2Int posOnMap)
    {
        this.PosOnMap = posOnMap;
    }

    public void Move(Direction dir)
    {
        switch(dir)
        {
        case Direction.R:
        {
            print("右！");
            PosOnMap += new Vector2Int(1, 0);
        }
        break;
        case Direction.L:
        {
            print("左！");
            PosOnMap += new Vector2Int(-1, 0);
        }
        break;
        case Direction.D:
        {
            print("下！");
            PosOnMap += new Vector2Int(0, 1);
        }
        break;
        case Direction.U:
        {
            print("上！");
            PosOnMap += new Vector2Int(0, -1);
        }
        break;
        }
    }


    void Update()
    {
        
    }
}
