using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator
{
    public GeneratedMap Generate()
    {
        var map = new GeneratedMap.MapInTheMaking();
        //雑
        map.tiles = Enumerable.Repeat(
                        Enumerable.Repeat(
                            TileType.Aisle,
                            5
                        ).ToList(),
                        5
                    ).ToList();
        return new GeneratedMap(map);
    }
}
