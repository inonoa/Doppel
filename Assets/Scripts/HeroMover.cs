using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using DG.Tweening;

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

    public void TryMove(Direction dir)
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
        DOVirtual.DelayedCall(2f, () => {
            _ActionCompleted = true;
            print("主人公移動終了");
        });
    }


    void Update()
    {
        //
    }
}
