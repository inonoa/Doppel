using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Aisle, Wall, Stair
}

public static class TileTypeExtension
{
    public static char ToDebugChar(this TileType type)
    {
        switch (type)
        {
        case TileType.Aisle: return ' ';
        case TileType.Wall:  return '■';
        case TileType.Stair: return 'L';
        default:             return 'X';
        }
    }
}
