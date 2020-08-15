using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;
using UnityEngine.UI;

public class TurnController : SerializedMonoBehaviour
{
    FloorStatus status;
    [SerializeField] FloorGenerator floorGenerator;
    [SerializeField] IMapView view;
    [SerializeField] Text dieText;

    void Start()
    {
        status = floorGenerator.Generate();
        view.SetStatus(status);
        dieText.gameObject.SetActive(false);

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
        int turn = 0;
        HeroMover.Direction? heroDir = GetInput();

        while(true)
        {
            turn ++;
            print($"ターン{turn}開始！");

            yield return new WaitUntil(() => (heroDir = GetInput()) != null && status.hero.CanMove(heroDir.Value));

            status.hero.Move(heroDir.Value);
            status.doppels.ForEach(dp => {
                dp.Move();
            });

            yield return new WaitUntil(() => {
                return status.hero.ActionCompleted
                    && status.doppels.All(dp => dp.ActionCompleted);
            });

            if(status.hero.GetView().Any(pos => status.doppels.Any(dp => pos == dp.PosOnMap)))
            {
                dieText.gameObject.SetActive(true);
                yield break;
            }
        }
    }
}

public interface IUnderTurns
{
    bool ActionCompleted{ get; }
}

public interface IMapView
{
    void SetStatus(FloorStatus status);
}
