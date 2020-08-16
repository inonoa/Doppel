using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class HeroView : MonoBehaviour
{
    SimpleAnimator anim;
    FloorStatus status;
    ViewParams params_;

    public IObservable<Unit> Move(HeroMover.Direction dir)
    {
        Subject<Unit> finished = new Subject<Unit>();
        transform.DOLocalMove(
            Pos(status, params_.TileSize),
            0.5f
        )
        .SetEase(Ease.InOutSine)
        .onComplete += () => finished.OnNext(Unit.Default);
        anim.Play("move" + TriggerStr(dir));
        return finished;
    }

    Vector3 Pos(FloorStatus status, float tileSize)
    {
        float x = (- (status.map.Width - 1) / 2f + status.hero.PosOnMap.x) * tileSize;
        float z = (  (status.map.Height - 1) / 2f - status.hero.PosOnMap.y) * tileSize;
        return new Vector3(x, 0, z);
    }

    string TriggerStr(HeroMover.Direction dir)
    {
        //ToSting()と同じか
        switch(dir)
        {
        case HeroMover.Direction.R: return "R";
        case HeroMover.Direction.U: return "U";
        case HeroMover.Direction.L: return "L";
        case HeroMover.Direction.D: return "D";
        default: return "";
        }
    }

    void Awake()
    {
        anim = GetComponent<SimpleAnimator>();
    }

    public void Init(FloorStatus status, ViewParams params_, HeroMover.Direction dir)
    {
        this.status = status;
        this.params_ = params_;
        this.transform.position = Pos(status, params_.TileSize);
        anim.Play("move" + TriggerStr(dir));
    }


    void Update()
    {
        
    }
}
