using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Linq;
using Random = UnityEngine.Random;

public class HeroMover : MonoBehaviour, IUnderTurns, IOnMap
{
    public enum Direction{ L, R, U, D }

    [ReadOnly] [SerializeField] Direction direction;
    [SerializeField] HeroView view;

    int viewLength = 3;
    public Vector2Int[] GetView()
    {
        return Enumerable.Range(1, viewLength)
               .Select(i => PosOnMap + ToVec(direction) * i)
               .TakeWhile(pos => pos.x > -1 && pos.x < status.map.Width && pos.y > -1 && pos.y < status.map.Height)
               .TakeWhile(pos => status.map.GetTile(pos) != TileType.Wall)
               .ToArray();
    }

    [field: SerializeField] [field: LabelText("Position On Map")] [field: ReadOnly]
    public Vector2Int PosOnMap{ get; private set; }

    public bool ActionCompleted => _ActionCompleted;
    bool _ActionCompleted = true;

    FloorStatus status;

    public void Init(Vector2Int posOnMap, FloorStatus status, ViewParams viewParams)
    {
        this.PosOnMap = posOnMap;
        this.status = status;
        this.direction = (Direction) Random.Range(0, 3);
        this.view.Init(this, status, viewParams, this.direction);

        SeenTilesCheck();
    }

    public bool CanMove(Direction dir)
    {
        Vector2Int next;
        switch(dir)
        {
            case Direction.R: next = PosOnMap + new Vector2Int(1,  0);  break;
            case Direction.L: next = PosOnMap + new Vector2Int(-1, 0);  break;
            case Direction.U: next = PosOnMap + new Vector2Int(0,  -1); break;
            case Direction.D: next = PosOnMap + new Vector2Int(0,  1);  break;
            default:          next = new Vector2Int(-1, -1);            break;
        }
        //マップ外(端が床にならないから意味ない気がするが)
        if(next.x < 0 || next.x >= status.map.Width)  return false;
        if(next.y < 0 || next.y >= status.map.Height) return false;
        //壁破壊不可能
        if(status.map.Tiles[next.y][next.x] == TileType.Wall) return false;
        //ドッペルに重ならない
        if(status.doppels.Any(dp => next == dp.PosOnMap)) return false;

        return true;
    }

    Vector2Int ToVec(Direction dir)
    {
        if(dir == Direction.R) return new Vector2Int(1, 0);
        if(dir == Direction.U) return new Vector2Int(0, -1);
        if(dir == Direction.L) return new Vector2Int(-1, 0);
                               return new Vector2Int(0, 1);
    }

    public void Move(Direction dir)
    {
        PosOnMap += ToVec(dir);
        _ActionCompleted = false;
        this.direction = dir;

        SeenTilesCheck();

        view.Move(dir)
        .Subscribe(_ =>
        {
            _ActionCompleted = true;
        });
    }

    void SeenTilesCheck()
    {
        GridUnit currentRoomGrid = status.map.Grids.FirstOrDefault(grid => 
        {
            bool xInside = grid.Room.Left <= PosOnMap.x && PosOnMap.x <= grid.Room.Right;
            bool yInside = grid.Room.Up   <= PosOnMap.y && PosOnMap.y <= grid.Room.Down;
            return xInside && yInside;
        });
        if(currentRoomGrid != null)
        {
            for(int y = currentRoomGrid.Room.Up - 1; y <= currentRoomGrid.Room.Down + 1; y ++)
            {
                for(int x = currentRoomGrid.Room.Left - 1; x <= currentRoomGrid.Room.Right + 1; x ++)
                {
                    status.seen[y][x] = true;
                }
            }
        }
        else
        {
            foreach(int y in new[] {PosOnMap.y - 1, PosOnMap.y, PosOnMap.y + 1})
            {
                foreach(int x in new[] {PosOnMap.x - 1, PosOnMap.x, PosOnMap.x + 1})
                {
                    status.seen[y][x] = true;
                }
            }
        }
    }


    void Update()
    {
        //
    }
}
