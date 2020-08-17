using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class InstructionSceneController : MonoBehaviour
{
    [SerializeField] DangeonController dangeonController;

    InstructionSceneView view;

    bool canClick = false;

    void Start()
    {
        view = GetComponent<InstructionSceneView>();
    }

    public void Enter()
    {
        view.Enter()
        .Subscribe(_ => 
        {
            canClick = true;
        });
    }
    
    public void Exit()
    {
        view.Exit()
        .Subscribe(_ => 
        {
            dangeonController.Enter();
        });
    }


    void Update()
    {
        if(canClick && Input.GetMouseButtonDown(0))
        {
            canClick = false;
            Exit();
        }
    }
}
