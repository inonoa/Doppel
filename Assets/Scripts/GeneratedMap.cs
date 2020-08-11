using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneratedMap
{
    TileType[][] _Tiles;
    public IReadOnlyList<IReadOnlyList<TileType>> Tiles => _Tiles;

    public GeneratedMap(MapInTheMaking madeMap)
    {
        _Tiles = madeMap.tiles
                 .Select(row => row.ToArray())
                 .ToArray();
    }

    public class MapInTheMaking
    {
        public List<List<TileType>> tiles = new List<List<TileType>>();
    }
}
