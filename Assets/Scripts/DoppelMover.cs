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
        //時計回りに二周探索
        //一周目、通路があればそこ
        for(int i = (int) lastDir; i < ((int) lastDir) + 4; i ++)
        {
            Vector2Int forward = Forward((Dir)(i % 4));
            if(status.hero.PosOnMap == forward) continue;
            if(status.doppels.Any(dp => forward == dp.PosOnMap)) continue;
            if(status.map.GetTile(forward) == TileType.Wall) continue;

            if(status.map.GetTile(forward) == TileType.Aisle)
            {
                lastDir = (Dir)(i % 4);
                return forward;
            }
        }
        //二周目、部屋内の床があればそこ
        for(int i = (int) lastDir; i < ((int) lastDir) + 4; i ++)
        {
            Vector2Int forward = Forward((Dir)(i % 4));
            if(status.hero.PosOnMap == forward) continue;
            if(status.doppels.Any(dp => forward == dp.PosOnMap)) continue;
            if(status.map.GetTile(forward) == TileType.Wall) continue;
            
            if(status.map.GetTile(forward) == TileType.RoomFloor
            || status.map.GetTile(forward) == TileType.Stair)
            {
                lastDir = (Dir)(i % 4);
                return forward;
            }
        }
        //どこにも行けなければ足踏み
        return PosOnMap;
    }

    Vector2Int Forward(Dir dir)
    {
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
