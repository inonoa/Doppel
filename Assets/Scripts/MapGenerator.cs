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
        var map = FilledMap(mapSize, TileType.Wall);

        HashSet<DividedGrid> divided = DivideGrid(map);

        CombineGrids(divided);
        CombineGrids(divided);
        CombineGrids(divided);
        
        foreach(DividedGrid gridUnit in divided)
        {
            // for(int i = gridUnit.leftUpBound.x; i < gridUnit.rightDownBound.x + 1; i ++)
            // {
            //     map.tiles[gridUnit.leftUpBound.y][i] = TileType.Stair;
            //     map.tiles[gridUnit.rightDownBound.y][i] = TileType.Stair;
            // }
            // for(int i = gridUnit.leftUpBound.y; i < gridUnit.rightDownBound.y + 1; i ++)
            // {
            //     map.tiles[i][gridUnit.leftUpBound.x] = TileType.Stair;
            //     map.tiles[i][gridUnit.rightDownBound.x] = TileType.Stair;
            // }

            //デバッグここまで

            Vector2Int lu = new Vector2Int();
            lu.x = Random.Range(gridUnit.leftUpBound.x + 1, gridUnit.rightDownBound.x - 3);
            lu.y = Random.Range(gridUnit.leftUpBound.y + 1, gridUnit.rightDownBound.y - 3);
            
            Vector2Int rd = new Vector2Int();
            rd.x = Random.Range(lu.x + 2, gridUnit.rightDownBound.x - 1);
            rd.y = Random.Range(lu.y + 2, gridUnit.rightDownBound.y - 1);

            for(int i = lu.x; i < rd.x + 1; i++)
            {
                for(int j = lu.y; j < rd.y + 1; j ++)
                {
                    map.tiles[j][i] = TileType.Aisle;
                }
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
                map.tiles.Last().Add(type);
            }
        }
        return map;
    }

    HashSet<DividedGrid> DivideGrid(MapInTheMaking map)
    {
        Vector2Int num_grid = new Vector2Int(map.tiles[0].Count / gridSize.x, map.tiles.Count / gridSize.y);

        var ans = new HashSet<DividedGrid>();

        int prev_right = map.tiles[0].Count;
        for(int i = num_grid.x - 1; i > -1; i --)
        {
            int left = (int)Mathf.Lerp(0, map.tiles[0].Count, i / (float)num_grid.x);

            int prev_down = map.tiles.Count;
            for(int j = num_grid.y - 1; j > -1; j --)
            {
                int up = Mathf.RoundToInt(Mathf.Lerp(0, map.tiles.Count, j / (float)num_grid.y));
                ans.Add(new DividedGrid(new Vector2Int(left, up), new Vector2Int(prev_right - 1, prev_down - 1)));
                prev_down = up;
            }
            prev_right = left;
        }

        return ans;
    }

    void CombineGrids(HashSet<DividedGrid> grids)
    {
        HashSet<DividedGrid> combined = new HashSet<DividedGrid>();
        HashSet<DividedGrid> combinedLarge = new HashSet<DividedGrid>();

        foreach(DividedGrid grid1 in grids)
        {
            foreach(DividedGrid grid2 in grids)
            {
                if(!combined.Contains(grid1) && !combined.Contains(grid2) && grid1.IsNextTo(grid2))
                {
                    if(Random.Range(0f, 1f) > 0.1f) continue;

                    combined.Add(grid1);
                    combined.Add(grid2);
                    combinedLarge.Add(grid1.Combine(grid2));
                }
            }
        }
        grids.RemoveWhere(grid => combined.Contains(grid));
        foreach(var grid in combinedLarge) grids.Add(grid);
    }

    public class DividedGrid
    {
        public readonly Vector2Int leftUpBound;
        public readonly Vector2Int rightDownBound;

        public DividedGrid(Vector2Int leftUpBound, Vector2Int rightDownBound)
        {
            (this.leftUpBound, this.rightDownBound) = (leftUpBound, rightDownBound);
        }

        //一辺を完全に共有しているときのみtrue
        public bool IsNextTo(DividedGrid other)
        {
            bool leftIsNext  = this.leftUpBound.x    == other.rightDownBound.x + 1;
            bool rightIsNext = this.rightDownBound.x == other.leftUpBound.x    - 1;
            if(leftIsNext || rightIsNext)
            {
                return this.leftUpBound.y    == other.leftUpBound.y
                    && this.rightDownBound.y == other.rightDownBound.y;
            }

            bool upIsNext   = this.leftUpBound.y    == other.rightDownBound.y + 1;
            bool downIsNext = this.rightDownBound.y == other.leftUpBound.y    - 1;
            if(upIsNext || downIsNext)
            {
                return this.leftUpBound.x    == other.leftUpBound.x
                    && this.rightDownBound.x == other.rightDownBound.x;
            }

            return false;
        }

        public DividedGrid Combine(DividedGrid other)
        {
            if(this.leftUpBound.x < other.leftUpBound.x
            || this.leftUpBound.y < other.leftUpBound.y)
            {
                return new DividedGrid(this.leftUpBound, other.rightDownBound);
            }
            else
            {
                return new DividedGrid(other.leftUpBound, this.rightDownBound);
            }
        }
    }
}
