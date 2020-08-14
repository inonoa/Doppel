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

        StartCoroutine(ProcTurns());
    }

    HeroMover.Direction? GetInput()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) return HeroMover.Direction.R;
        if(Input.GetKeyDown(KeyCode.LeftArrow )) return HeroMover.Direction.L;
        if(Input.GetKeyDown(KeyCode.UpArrow   )) return HeroMover.Direction.U;
        if(Input.GetKeyDown(KeyCode.DownArrow )) return HeroMover.Direction.D;
        return null;
    }

    IEnumerator ProcTurns()
    {
        HeroMover.Direction? heroDir = GetInput();

        while(true)
        {
            yield return new WaitUntil(() => (heroDir = GetInput()) != null);

            status.hero.TryMove(heroDir.Value);

            yield return null;
        }
    }
}
