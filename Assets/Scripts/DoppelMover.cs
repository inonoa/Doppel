using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Move()
    {
        _ActionCompleted = false;
        DOVirtual.DelayedCall(1f, () => {
            _ActionCompleted = true;
            print("移動終了");
        });
    }
}
