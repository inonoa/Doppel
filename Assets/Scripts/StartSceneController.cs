using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class StartSceneController : MonoBehaviour
{
    StartSceneView view;

    bool canClick = false;

    void Start()
    {
        view = GetComponent<StartSceneView>();
        Enter();
    }

    public void Enter()
    {
        DOVirtual.DelayedCall(1, () => 
        {
            view.Enter()
            .Subscribe(_ => 
            {
                canClick = true;
            });
        });
    }


    void Update()
    {
        if(canClick && Input.GetMouseButtonDown(0))
        {
            canClick = false;
            view.Exit()
            .Subscribe(_ => 
            {
                print("説明画面にいきたい");
            });
        }
    }
}
