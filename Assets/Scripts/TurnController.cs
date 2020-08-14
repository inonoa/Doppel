using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;

public class TurnController : SerializedMonoBehaviour
{
    FloorStatus status;
    [SerializeField] FloorGenerator floorGenerator;

    void Start()
    {
        status = floorGenerator.Generate();
        status.hero.OnStartMove.Subscribe(dir => {
            status.doppels.ForEach(dp => dp.Move());
        });
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) status.hero.TryMove(HeroMover.Direction.R);
        if(Input.GetKeyDown(KeyCode.LeftArrow )) status.hero.TryMove(HeroMover.Direction.L);
        if(Input.GetKeyDown(KeyCode.UpArrow   )) status.hero.TryMove(HeroMover.Direction.U);
        if(Input.GetKeyDown(KeyCode.DownArrow )) status.hero.TryMove(HeroMover.Direction.D);
    }
}
