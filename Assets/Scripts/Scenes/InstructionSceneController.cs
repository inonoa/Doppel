using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class InstructionSceneController : MonoBehaviour
{
    InstructionSceneView view;

    void Start()
    {
        view = GetComponent<InstructionSceneView>();
    }

    public void Enter()
    {
        view.Enter()
        .Subscribe(_ => 
        {
            print("a");
        });
    }
    
    public void Exit()
    {
        view.Exit()
        .Subscribe(_ => 
        {
            print("a");
        });
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Exit();
        }
    }
}
