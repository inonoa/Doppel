using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class StartSceneView : MonoBehaviour
{
    [SerializeField] Image titleImg;
    [SerializeField] Image titleShadowImg;
    [SerializeField] Image frameImg;
    [SerializeField] Image rubyImg;
    [SerializeField] Image bgImg;
    [SerializeField] Image clickImg;

    void Start()
    {
        foreach(Image img in new[] {titleImg, titleShadowImg, frameImg, rubyImg, /*bgImg, */clickImg})
        {
            img.color = new Color(1, 1, 1, 0);
        }
    }

    public IObservable<Unit> Enter()
    {
        bgImg.color = Color.white;

        Sequence enterSeq = DOTween.Sequence();

        float titleFadeSec =    0.8f;
        enterSeq.Append(titleImg      .DOFade(1, titleFadeSec));
        enterSeq.Join(  titleShadowImg.DOFade(1, titleFadeSec));

        enterSeq.AppendInterval(0.8f);

        float othersFadeSec =   0.8f;
        enterSeq.Append(frameImg.DOFade(1, othersFadeSec));
        enterSeq.Join(  rubyImg .DOFade(1, othersFadeSec));
        enterSeq.Join(  clickImg.DOFade(1, othersFadeSec));

        Subject<Unit> completed = new Subject<Unit>();
        enterSeq.onComplete += () =>
        {
            completed.OnNext(Unit.Default);
        };
        return completed;
    }

    public IObservable<Unit> Exit()
    {
        var seq = DOTween.Sequence();

        float othersFadeSec =     0.5f;
        seq.Append(titleImg.DOFade(0, othersFadeSec));
        seq.Join(  frameImg.DOFade(0, othersFadeSec));
        seq.Join(  clickImg.DOFade(0, othersFadeSec));

        seq.AppendInterval(       0.8f);

        float shadowRubyFadeSec = 0.5f;
        seq.Append(titleShadowImg.DOFade(0, shadowRubyFadeSec));
        seq.Join(  rubyImg       .DOFade(0, shadowRubyFadeSec));

        Subject<Unit> completed = new Subject<Unit>();
        seq.onComplete += () =>
        {
            bgImg.color = new Color(1, 1, 1, 0);
            completed.OnNext(Unit.Default);
        };
        return completed;
    }


    void Update()
    {
        
    }
}
