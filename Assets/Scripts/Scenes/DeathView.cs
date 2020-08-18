using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.UI;

public class DeathView : MonoBehaviour
{
    [SerializeField] Image noiseImg;

    void Start()
    {
        
    }

    public IObservable<Unit> Enter()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(
            DOTween.To(
                () => noiseImg.material.GetFloat("_Threshold"),
                v  => noiseImg.material.SetFloat("_Threshold", v),
                1,
                2.5f
            )
            .SetEase(Ease.InOutSine)
        );

        return seq.CompletedAsObservable();
    }

    public IObservable<Unit> Exit()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(
            DOTween.To(
                () => noiseImg.material.GetFloat("_Threshold"),
                v  => noiseImg.material.SetFloat("_Threshold", v),
                0,
                1.25f
            )
            .SetEase(Ease.InOutSine)
        );

        return seq.CompletedAsObservable();
    }
}
