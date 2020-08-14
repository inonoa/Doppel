using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMover : MonoBehaviour
{
    public enum Direction{ L, R, U, D }

    public void Move(Direction dir)
    {
        switch(dir)
        {
        case Direction.R:
        {
            print("右！");
        }
        break;
        case Direction.L:
        {
            print("左！");
        }
        break;
        case Direction.D:
        {
            print("下！");
        }
        break;
        case Direction.U:
        {
            print("上！");
        }
        break;
        }
    }


    void Update()
    {
        
    }
}
