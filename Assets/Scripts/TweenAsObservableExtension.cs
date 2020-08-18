using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public static class TweenAsObservableExtension
{
    public static IObservable<Unit> CompletedAsObservable(this Tween tween)
    {
        var completed = new Subject<Unit>();
        tween.onComplete += () => completed.OnNext(Unit.Default);
        return completed;
    }
}
