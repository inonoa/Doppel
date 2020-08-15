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

    void Start()
    {
        InitFloor(0);
        dieText.gameObject.SetActive(false);
    }

    void InitFloor(int floor)
    {
        var turnController = Instantiate(turnControllerPrefab);
        turnController.Init(floor, dieText, mapDebugView);
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


    void Update()
    {
        
    }
}
