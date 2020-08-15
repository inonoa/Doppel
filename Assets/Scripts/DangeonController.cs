using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DangeonController : SerializedMonoBehaviour
{
    [SerializeField] TurnController turnControllerPrefab;
    [SerializeField] int floor = 50;
    [SerializeField] Text dieText;
    [SerializeField] IMapView mapDebugView;

    void Start()
    {
        Instantiate(turnControllerPrefab).Init(floor, dieText, mapDebugView);
    }


    void Update()
    {
        
    }
}
