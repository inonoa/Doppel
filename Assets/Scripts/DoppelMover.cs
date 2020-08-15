using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoppelMover : MonoBehaviour, IUnderTurns
{
    public bool ActionCompleted => _ActionCompleted;
    bool _ActionCompleted = true;

    HeroMover hero;
    public void Init(HeroMover hero)
    {
        this.hero = hero;
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
