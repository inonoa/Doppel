using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System.Linq;

public class DoppelMover : MonoBehaviour, IUnderTurns
{
    public bool ActionCompleted => _ActionCompleted;
    bool _ActionCompleted = true;

    public Vector2Int PosOnMap{ get; private set; }

    FloorStatus status;
    public void Init(FloorStatus status, Vector2Int posOnMap)
    {
        this.status = status;
        this.PosOnMap = posOnMap;
        this.lastDir = (Dir) Random.Range(0, 3);
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    enum Dir{ R = 0, U = 1, L = 2, D = 3 }

    Dir lastDir;

    public void Move()
    {
        Vector2Int next = NextPos();
        PosOnMap = next;

        _ActionCompleted = false;
        DOVirtual.DelayedCall(0.3f, () => {
            _ActionCompleted = true;
        });
    }

    Vector2Int NextPos()
    {
        //優先順位: 正面の通路>横の通路>正面の部屋床>横の部屋床>後ろの通路>後ろの部屋床
        //どこにも行けなければ足踏み
        Dir[] dirs = DirPriority(lastDir);
        Vector2Int[] priority = DirPriority(lastDir).Select(Forward).ToArray();

        if(CanMove(priority[0]) &&  status.map.GetTile(priority[0]) == TileType.Aisle ){ lastDir = dirs[0]; return priority[0]; }
        if(CanMove(priority[1]) &&  status.map.GetTile(priority[1]) == TileType.Aisle ){ lastDir = dirs[1]; return priority[1]; }
        if(CanMove(priority[2]) &&  status.map.GetTile(priority[2]) == TileType.Aisle ){ lastDir = dirs[2]; return priority[2]; }
        if(CanMove(priority[0]) && (status.map.GetTile(priority[0]) == TileType.RoomFloor
                                ||  status.map.GetTile(priority[0]) == TileType.Stair)){ lastDir = dirs[0]; return priority[0]; }
        if(CanMove(priority[1]) && (status.map.GetTile(priority[1]) == TileType.RoomFloor
                                ||  status.map.GetTile(priority[1]) == TileType.Stair)){ lastDir = dirs[1]; return priority[1]; }
        if(CanMove(priority[2]) && (status.map.GetTile(priority[2]) == TileType.RoomFloor
                                ||  status.map.GetTile(priority[2]) == TileType.Stair)){ lastDir = dirs[2]; return priority[2]; }
        if(CanMove(priority[3]) &&  status.map.GetTile(priority[3]) == TileType.Aisle ){ lastDir = dirs[3]; return priority[3]; }
        if(CanMove(priority[3]) && (status.map.GetTile(priority[3]) == TileType.RoomFloor
                                ||  status.map.GetTile(priority[3]) == TileType.Stair)){ lastDir = dirs[3]; return priority[3]; }

        return PosOnMap;
    }

    bool CanMove(Vector2Int nextPos)
    {
        if(status.hero.PosOnMap == nextPos) return false;
        if(status.doppels.Any(dp => nextPos == dp.PosOnMap)) return false;
        if(status.map.GetTile(nextPos) == TileType.Wall) return false;
        
        return true;
    }

    Dir[] DirPriority(Dir first)
    {
        Dir[] ans = new Dir[4];
        ans[0] = first; //正面
        ans[1] = (Dir) (((int) first + 1) % 4); //自分から見て左
        ans[2] = (Dir) (((int) first + 3) % 4); //自分から見て右
        ans[3] = (Dir) (((int) first + 2) % 4); //自分から見て後ろ

        return ans;
    }

    Vector2Int Forward(Dir dir)
    {
        Debug.Log(dir);
        switch(dir)
        {
        case Dir.R: return PosOnMap + new Vector2Int(1, 0);
        case Dir.U: return PosOnMap + new Vector2Int(0, -1);
        case Dir.L: return PosOnMap + new Vector2Int(-1, 0);
        case Dir.D: return PosOnMap + new Vector2Int(0, 1);
        default:    return new Vector2Int(-1, -1);
        }
    }
}
