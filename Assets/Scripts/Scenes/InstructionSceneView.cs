using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class InstructionSceneView : MonoBehaviour
{
    [SerializeField] SwitchedSpritesPair spritesW;
    [SerializeField] SwitchedSpritesPair spritesA;
    [SerializeField] SwitchedSpritesPair spritesS;
    [SerializeField] SwitchedSpritesPair spritesD;
    public IObservable<Unit> Enter()
    {
        var completed = Observable.Timer(TimeSpan.FromSeconds(1))
                        .Select(_ => Unit.Default);

        return completed;
    }

    public IObservable<Unit> Exit()
    {
        var completed = Observable.Timer(TimeSpan.FromSeconds(1))
                        .Select(_ => Unit.Default);

        return completed;
    }

    void Start()
    {
        
    }


    void Update()
    {
        spritesW.On = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        spritesA.On = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        spritesS.On = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        spritesD.On = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }
}
