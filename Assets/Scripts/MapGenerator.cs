using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using MapInTheMaking = GeneratedMap.MapInTheMaking;


[CreateAssetMenu(menuName = "ScriptableObject/MapGenerator", fileName = "MapGenerator", order = 5)]
public class MapGenerator : ScriptableObject
{
    [SerializeField] Vector2Int mapSize = new Vector2Int(30, 30);
    [SerializeField] Vector2Int gridSize = new Vector2Int(6, 6);

    public GeneratedMap Generate()
    {
        var map = FilledMap(mapSize, TileType.Aisle);

        DividedGrid[,] divided = DivideGrid(map);
        
        foreach(DividedGrid gridUnit in divided)
        {
            for(int i = gridUnit.leftUpBound.x; i < gridUnit.leftUpBound.x + gridUnit.size.x; i ++)
            {
                map.tiles[gridUnit.leftUpBound.y][i] = TileType.Wall;
                map.tiles[gridUnit.leftUpBound.y + gridUnit.size.y - 1][i] = TileType.Wall;
            }
            for(int i = gridUnit.leftUpBound.y; i < gridUnit.leftUpBound.y + gridUnit.size.y; i ++)
            {
                map.tiles[i][gridUnit.leftUpBound.x] = TileType.Wall;
                map.tiles[i][gridUnit.leftUpBound.x + gridUnit.size.x - 1] = TileType.Wall;
            }
        }

        return new GeneratedMap(map);
    }

    MapInTheMaking FilledMap(Vector2Int size, TileType type)
    {
        var map = new MapInTheMaking();
        foreach(int i in Enumerable.Range(0, mapSize.y))
        {
            map.tiles.Add(new List<TileType>());
            foreach (int j in Enumerable.Range(0, mapSize.x))
            {
                map.tiles.Last().Add(TileType.Aisle);
            }
        }
        return map;
    }

    DividedGrid[,] DivideGrid(MapInTheMaking map)
    {
        Vector2Int num_grid = new Vector2Int(map.tiles[0].Count / gridSize.x, map.tiles.Count / gridSize.y);

        var ans = new DividedGrid[num_grid.x, num_grid.y];

        int prev_right = map.tiles[0].Count;
        for(int i = num_grid.x - 1; i > -1; i --)
        {
            int left = (int)Mathf.Lerp(0, map.tiles[0].Count, i / (float)num_grid.x);

            int prev_down = map.tiles.Count;
            for(int j = num_grid.y - 1; j > -1; j --)
            {
                int up = Mathf.RoundToInt(Mathf.Lerp(0, map.tiles.Count, j / (float)num_grid.y));
                ans[i, j] = new DividedGrid(new Vector2Int(left, up), new Vector2Int(prev_right - left, prev_down - up));
                prev_down = up;
            }
            prev_right = left;
        }

        return ans;
    }

    public class DividedGrid
    {
        public readonly Vector2Int leftUpBound;
        public readonly Vector2Int size;

        public DividedGrid(Vector2Int leftUpBound, Vector2Int size)
        {
            (this.leftUpBound, this.size) = (leftUpBound, size);
        }
    }
}
