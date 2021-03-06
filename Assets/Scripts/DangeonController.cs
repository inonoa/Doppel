﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UniRx;
using DG.Tweening;

public class DangeonController : SerializedMonoBehaviour
{
    [SerializeField] TurnController turnControllerPrefab;
    TurnController currentTurnController;
    [SerializeField] IMapView mapDebugView;
    [SerializeField] CameraMover cameraMover;
    [SerializeField] TransitionView transitionView;
    [SerializeField] DeathSceneController deathSceneController;

    void Start()
    {
        mapDebugView.SetActive(false);
    }

    void InitFloor(int floor)
    {
        currentTurnController = Instantiate(turnControllerPrefab);
        currentTurnController.Init(floor, mapDebugView, cameraMover);
        currentTurnController.NextFloor.Subscribe(_ =>
        {
            currentTurnController.AcceptsInput = false;
            GoDownstairs(floor + 1);
        });
        currentTurnController.Died.Subscribe(_ => 
        {
            deathSceneController.Enter(floor)
            .Subscribe(__ =>
            {
                Destroy(currentTurnController.gameObject);
            });
        });
    }

    public void Enter()
    {
        InitFloor(0);
        mapDebugView.SetActive(true);
        currentTurnController.AcceptsInput = true;
    }

    void GoDownstairs(int nextFloor)
    {
        transitionView.Enter(nextFloor)
        .Subscribe(_ => 
        {
            Destroy(currentTurnController.gameObject);
            InitFloor(nextFloor);
            DOVirtual.DelayedCall(1f, () => 
            {
                transitionView.Exit()
                .Subscribe(__ => currentTurnController.AcceptsInput = true);
            });
        });
    }


    void Update()
    {
        
    }
}
