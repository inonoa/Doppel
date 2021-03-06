﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class TurnController : SerializedMonoBehaviour
{
    FloorStatus status;
    [SerializeField] FloorGenerator floorGenerator;
    IMapView debugView;
    [SerializeField] IMapView view;
    [SerializeField] ViewParams viewParams;

    Subject<Unit> _NextFloor = new Subject<Unit>();
    public IObservable<Unit> NextFloor => _NextFloor;

    Subject<Unit> _Died = new Subject<Unit>();
    public IObservable<Unit> Died => _Died;

    public bool AcceptsInput{ get; set; } = false;

    public void Init(int floor, IMapView debugView, CameraMover cameraMover)
    {
        this.debugView = debugView;
        status = floorGenerator.Generate(floor, viewParams);
        this.debugView.SetStatus(status);
        this.view.SetStatus(status);
        cameraMover.Init(status);

        StartCoroutine(ProcTurns());
    }

    HeroMover.Direction? GetInput()
    {
        if(! AcceptsInput) return null;

        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) return HeroMover.Direction.R;
        if(Input.GetKeyDown(KeyCode.LeftArrow ) || Input.GetKeyDown(KeyCode.A)) return HeroMover.Direction.L;
        if(Input.GetKeyDown(KeyCode.UpArrow   ) || Input.GetKeyDown(KeyCode.W)) return HeroMover.Direction.U;
        if(Input.GetKeyDown(KeyCode.DownArrow ) || Input.GetKeyDown(KeyCode.S)) return HeroMover.Direction.D;
        return null;
    }

    IEnumerator ProcTurns()
    {
        int turn = 0;
        HeroMover.Direction? heroDir = GetInput();

        while(true)
        {
            turn ++;

            yield return new WaitUntil(() => (heroDir = GetInput()) != null && status.hero.CanMove(heroDir.Value));

            status.hero.Move(heroDir.Value);
            status.doppels.ForEach(dp => {
                dp.Move();
            });

            yield return new WaitUntil(() => {
                return status.hero.ActionCompleted
                    && status.doppels.All(dp => dp.ActionCompleted);
            });

            if(status.hero.GetView().Any(pos => status.doppels.Any(dp => pos == dp.PosOnMap)))
            {
                _Died.OnNext(Unit.Default);
                yield break;
            }

            if(status.map.GetTile(status.hero.PosOnMap) == TileType.Stair)
            {
                _NextFloor.OnNext(Unit.Default);
                yield break;
            }
        }
    }
}

public interface IUnderTurns
{
    bool ActionCompleted{ get; }
}

public interface IMapView
{
    void SetStatus(FloorStatus status);
    void SetActive(bool active);
}
