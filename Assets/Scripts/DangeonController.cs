using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UniRx;

public class DangeonController : SerializedMonoBehaviour
{
    [SerializeField] TurnController turnControllerPrefab;
    [SerializeField] Text dieText;
    [SerializeField] IMapView mapDebugView;
    [SerializeField] CameraMover cameraMover;

    void Start()
    {
        dieText.gameObject.SetActive(false);
        mapDebugView.SetActive(false);
    }

    void InitFloor(int floor)
    {
        var turnController = Instantiate(turnControllerPrefab);
        turnController.Init(floor, dieText, mapDebugView, cameraMover);
        turnController.NextFloor.Subscribe(_ =>
        {
            Destroy(turnController.gameObject);
            InitFloor(floor + 1);
        });
        turnController.Died.Subscribe(_ => 
        {
            Destroy(turnController.gameObject);
            InitFloor(0);
        });
    }

    public void Enter()
    {
        InitFloor(0);
        mapDebugView.SetActive(true);
    }


    void Update()
    {
        
    }
}
