using System;
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
    [SerializeField] Text dieText;
    [SerializeField] IMapView mapDebugView;
    [SerializeField] CameraMover cameraMover;
    [SerializeField] TransitionView transitionView;

    void Start()
    {
        dieText.gameObject.SetActive(false);
        mapDebugView.SetActive(false);
    }

    void InitFloor(int floor)
    {
        currentTurnController = Instantiate(turnControllerPrefab);
        currentTurnController.Init(floor, dieText, mapDebugView, cameraMover);
        currentTurnController.NextFloor.Subscribe(_ =>
        {
            Destroy(currentTurnController.gameObject);
            GoDownstairs(floor + 1);
        });
        currentTurnController.Died.Subscribe(_ => 
        {
            Destroy(currentTurnController.gameObject);
            InitFloor(0);
        });
    }

    public void Enter()
    {
        InitFloor(0);
        mapDebugView.SetActive(true);
    }

    void GoDownstairs(int nextFloor)
    {
        transitionView.Enter()
        .Subscribe(_ => 
        {
            InitFloor(nextFloor);
            DOVirtual.DelayedCall(1f, () => transitionView.Exit());
        });
    }


    void Update()
    {
        
    }
}
