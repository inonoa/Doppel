using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Linq;

public class HeroMover : MonoBehaviour, IUnderTurns
{
    public enum Direction{ L, R, U, D }

    [field: SerializeField] [field: LabelText("Position On Map")] [field: ReadOnly]
    public Vector2Int PosOnMap{ get; private set; }

    public bool ActionCompleted => _ActionCompleted;
    bool _ActionCompleted = true;

    FloorStatus status;

    public void Init(Vector2Int posOnMap, FloorStatus status)
    {
        this.PosOnMap = posOnMap;
        this.status = status;
    }

    Subject<Direction> _OnStartMove = new Subject<Direction>();
    public IObservable<Direction> OnStartMove => _OnStartMove;

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

    public void Move(Direction dir)
    {
        switch(dir)
        {
        case Direction.R:
        {
            PosOnMap += new Vector2Int(1, 0);
        }
        break;
        case Direction.L:
        {
            PosOnMap += new Vector2Int(-1, 0);
        }
        break;
        case Direction.D:
        {
            PosOnMap += new Vector2Int(0, 1);
        }
        break;
        case Direction.U:
        {
            PosOnMap += new Vector2Int(0, -1);
        }
        break;
        }
        _ActionCompleted = false;
        DOVirtual.DelayedCall(0.5f, () => {
            _ActionCompleted = true;
        });
    }


    void Update()
    {
        //
    }
}
