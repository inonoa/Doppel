using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Linq;

public class InstructionSceneView : MonoBehaviour
{
    Image[] PurposeImgs => new[] { purposeImg, doppelwo_orirukoto_Img, minaideImg };
    [SerializeField, FoldoutGroup("もくてき")] Image purposeImg;
    [SerializeField, FoldoutGroup("もくてき")] Image doppelwo_orirukoto_Img;
    [SerializeField, FoldoutGroup("もくてき")] Image minaideImg;

    Image[] MeansImgs => new[] {
        meansImg, idousuruImg,
        spritesW.Renderer, spritesA.Renderer, spritesS.Renderer, spritesD.Renderer,
        wasdImg
    };
    [SerializeField, FoldoutGroup("しゅだん")] Image meansImg;
    [SerializeField, FoldoutGroup("しゅだん")] Image idousuruImg;

    [SerializeField, FoldoutGroup("WASD")] SwitchedSpritesPair spritesW;
    [SerializeField, FoldoutGroup("WASD")] SwitchedSpritesPair spritesA;
    [SerializeField, FoldoutGroup("WASD")] SwitchedSpritesPair spritesS;
    [SerializeField, FoldoutGroup("WASD")] SwitchedSpritesPair spritesD;
    [SerializeField, FoldoutGroup("WASD")] Image wasdImg;

    [SerializeField] Image clickImg;
    [SerializeField] Image bgImg;

    public IObservable<Unit> Enter()
    {
        bgImg.color = Color.white;

        var seq = DOTween.Sequence();

        seq.AppendInterval(0);
        PurposeImgs
        .Select(img => img.DOFade(1, 0.8f))
        .ForEach(tw => seq.Join(tw));

        seq.AppendInterval(0.3f);

        seq.AppendInterval(0);
        MeansImgs
        .Select(img => img.DOFade(1, 0.8f))
        .ForEach(tw => seq.Join(tw));

        seq.AppendInterval(0.3f);

        seq.Join(clickImg.DOFade(1, 0.2f));

        var completed = new Subject<Unit>();
        seq.onComplete += () => completed.OnNext(Unit.Default);

        return completed;
    }

    public IObservable<Unit> Exit()
    {
        var seq = DOTween.Sequence();

        Image[] left = new[]{ minaideImg, wasdImg };

        seq.AppendInterval(0);
        PurposeImgs.Concat(MeansImgs).Append(clickImg)
        .Except(left)
        .Select(img => img.DOFade(0, 0.8f))
        .ForEach(tw => seq.Join(tw));

        seq.AppendInterval(0.5f);

        seq.AppendInterval(0);
        left
        .Select(img => img.DOFade(0, 0.5f))
        .ForEach(tw => seq.Join(tw));

        seq.Append(bgImg.DOFade(0, 0.3f));

        var completed = new Subject<Unit>();
        seq.onComplete += () => completed.OnNext(Unit.Default);

        return completed;
    }

    void Start()
    {
        PurposeImgs.Concat(MeansImgs).Append(clickImg).Append(bgImg)
        .ForEach(img => img.color = new Color(1, 1, 1, 0));
    }


    void Update()
    {
        spritesW.On = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        spritesA.On = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        spritesS.On = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        spritesD.On = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }
}
