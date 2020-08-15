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
    [SerializeField] ParamVec2Int mapSize;
    [SerializeField] ParamVec2Int gridSize;

    public GeneratedMap Generate(int floor)
    {
        var map = FilledMap(mapSize.Get(floor), TileType.Wall, floor);

        HashSet<GridUnit> divided = DivideGrid(map, floor);

        CombineGrids(divided);
        CombineGrids(divided);
        CombineGrids(divided);

        HashSet<Room> rooms = new HashSet<Room>();
        
        foreach(GridUnit gridUnit in divided)
        {
            //GridUnitDebug(gridUnit, map);

            Room room = gridUnit.CreateRoom();

            rooms.Add(room);

            room.Apply(map);
        }

        divided.ForEachCombi((gr1, gr2) => TryLayRoad(map, gr1, gr2));

        Room stairRoom = rooms.ElementAt(Random.Range(0, rooms.Count - 1));
        Vector2Int stairCoord = new Vector2Int(
                                    Random.Range(stairRoom.Left + 1, stairRoom.Right - 1),
                                    Random.Range(stairRoom.Up   + 1, stairRoom.Down  - 1)
                                );
        map.tiles[stairCoord.y][stairCoord.x] = TileType.Stair;

        return new GeneratedMap(map);
    }

    MapInTheMaking FilledMap(Vector2Int size, TileType type, int floor)
    {
        var map = new MapInTheMaking();
        foreach(int i in Enumerable.Range(0, mapSize.Get(floor).y))
        {
            map.tiles.Add(new List<TileType>());
            foreach (int j in Enumerable.Range(0, mapSize.Get(floor).x))
            {
                map.tiles.Last().Add(type);
            }
        }
        return map;
    }

    HashSet<GridUnit> DivideGrid(MapInTheMaking map, int floor)
    {
        Vector2Int num_grid = new Vector2Int(map.tiles[0].Count / gridSize.Get(floor).x, map.tiles.Count / gridSize.Get(floor).y);

        var ans = new HashSet<GridUnit>();

        int prev_right = map.tiles[0].Count;
        for(int i = num_grid.x - 1; i > -1; i --)
        {
            int left = (int)Mathf.Lerp(0, map.tiles[0].Count, i / (float)num_grid.x);

            int prev_down = map.tiles.Count;
            for(int j = num_grid.y - 1; j > -1; j --)
            {
                int up = Mathf.RoundToInt(Mathf.Lerp(0, map.tiles.Count, j / (float)num_grid.y));
                ans.Add(new GridUnit(new Vector2Int(left, up), new Vector2Int(prev_right - 1, prev_down - 1)));
                prev_down = up;
            }
            prev_right = left;
        }

        return ans;
    }

    void CombineGrids(HashSet<GridUnit> grids)
    {
        HashSet<GridUnit> combined = new HashSet<GridUnit>();
        HashSet<GridUnit> combinedLarge = new HashSet<GridUnit>();

        //自分自身とCombineし得るな（今は良いけど）
        foreach(GridUnit grid1 in grids)
        {
            foreach(GridUnit grid2 in grids)
            {
                if(!combined.Contains(grid1) && !combined.Contains(grid2) && grid1.CanCombine(grid2))
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

    void TryLayRoad(MapInTheMaking map, GridUnit src, GridUnit dst)
    {
        var dir = src.GetAdjacent(dst);
        switch(dir)
        {
        case GridUnit.Direction.Left:
            {
                int srcY = Random.Range(src.Room.Up, src.Room.Down);
                int dstY = Random.Range(dst.Room.Up, dst.Room.Down);
                int tateX = Random.Range(src.Room.Left - 1, dst.Room.Right + 1); //範囲無の場合無い……？ -> とりあえず部屋にへばりつく通路も許容
                LayHorizontalLine(map, srcY, src.Room.Left - 1, tateX);
                LayVerticalLine(map, tateX, srcY, dstY);
                LayHorizontalLine(map, dstY, tateX, dst.Room.Right + 1);
            }
            break;
        case GridUnit.Direction.Right:
            {
                int srcY = Random.Range(src.Room.Up, src.Room.Down);
                int dstY = Random.Range(dst.Room.Up, dst.Room.Down);
                int tateX = Random.Range(src.Room.Right + 1, dst.Room.Left - 1); //範囲無の場合無い……？ -> とりあえず部屋にへばりつく通路も許容
                LayHorizontalLine(map, srcY, src.Room.Right + 1, tateX);
                LayVerticalLine(map, tateX, srcY, dstY);
                LayHorizontalLine(map, dstY, tateX, dst.Room.Left - 1);
            }
            break;
        case GridUnit.Direction.Up:
            {
                int srcX = Random.Range(src.Room.Left, src.Room.Right);
                int dstX = Random.Range(dst.Room.Left, dst.Room.Right);
                int yokoY = Random.Range(src.Room.Up - 1, dst.Room.Down + 1); //範囲無の場合無い……？ -> とりあえず部屋にへばりつく通路も許容
                LayVerticalLine(map, srcX, src.Room.Up - 1, yokoY);
                LayHorizontalLine(map, yokoY, srcX, dstX);
                LayVerticalLine(map, dstX, yokoY, dst.Room.Down + 1);
            }
            break;
        case GridUnit.Direction.Down:
            {
                int srcX = Random.Range(src.Room.Left, src.Room.Right);
                int dstX = Random.Range(dst.Room.Left, dst.Room.Right);
                int yokoY = Random.Range(src.Room.Down + 1, dst.Room.Up - 1); //範囲無の場合無い……？ -> とりあえず部屋にへばりつく通路も許容
                LayVerticalLine(map, srcX, src.Room.Down + 1, yokoY);
                LayHorizontalLine(map, yokoY, srcX, dstX);
                LayVerticalLine(map, dstX, yokoY, dst.Room.Up - 1);
            }
            break;
        default:
            break;
        }
    }

    void LayVerticalLine(MapInTheMaking map, int x, int srcY, int dstY)
    {
        if(srcY > dstY)
        {
            for(int y = srcY; y >= dstY; y --)
            {
                map.tiles[y][x] = TileType.Aisle;
            }
        }
        if(srcY < dstY)
        {
            for(int y = srcY; y <= dstY; y ++)
            {
                map.tiles[y][x] = TileType.Aisle;
            }
        }
    }

    void LayHorizontalLine(MapInTheMaking map, int y, int srcX, int dstX)
    {
        if(srcX > dstX)
        {
            for(int x = srcX; x >= dstX; x --)
            {
                map.tiles[y][x] = TileType.Aisle;
            }
        }
        if(srcX < dstX)
        {
            for(int x = srcX; x <= dstX; x ++)
            {
                map.tiles[y][x] = TileType.Aisle;
            }
        }
    }


    void GridUnitDebug(GridUnit gridUnit, MapInTheMaking dst)
    {
        for(int i = gridUnit.leftUp.x; i < gridUnit.rightDown.x + 1; i ++)
        {
            dst.tiles[gridUnit.leftUp.y][i] = TileType.Stair;
            dst.tiles[gridUnit.rightDown.y][i] = TileType.Stair;
        }
        for(int i = gridUnit.leftUp.y; i < gridUnit.rightDown.y + 1; i ++)
        {
            dst.tiles[i][gridUnit.leftUp.x] = TileType.Stair;
            dst.tiles[i][gridUnit.rightDown.x] = TileType.Stair;
        }
    }
}
