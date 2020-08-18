using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class TransitionView : MonoBehaviour
{

    public IObservable<Unit> Enter()
    {
        return Observable.Timer(TimeSpan.FromSeconds(1))
               .Select(_ => Unit.Default);
    }

    public IObservable<Unit> Exit()
    {
        return new Subject<Unit>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
