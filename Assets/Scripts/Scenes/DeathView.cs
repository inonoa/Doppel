using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class DeathView : MonoBehaviour
{
    [SerializeField] Image noiseImg;
    [SerializeField] Image recordImg;
    [SerializeField] FloorNumbersView numbersView;
    [SerializeField] Button tweetButton;
    [SerializeField] Button retryButton;

    public IObservable<Unit> TweetButtonPushed => tweetButton.OnClickAsObservable();
    public IObservable<Unit> RetryButtonPushed => retryButton.OnClickAsObservable();

    void Start()
    {
        numbersView.Images.Append(recordImg).Append(tweetButton.image).Append(retryButton.image)
        .ForEach(img => img.DOFade(0, 0));
        noiseImg.material.SetFloat("_Threshold", 0);
    }

    public IObservable<Unit> Enter(int lastFloor)
    {
        numbersView.SetNumbers(lastFloor);
        numbersView.Images.ForEach(img => img.color = new Color(img.color.r, img.color.g, img.color.b, 0));

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

        seq.AppendInterval(0.3f);

        seq.AppendInterval(0);
        numbersView.Images.Append(recordImg).Append(tweetButton.image).Append(retryButton.image)
        .ForEach(img => seq.Join(img.DOFade(1, 0.3f)));

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
        numbersView.Images.Append(recordImg).Append(tweetButton.image).Append(retryButton.image)
        .ForEach(img => seq.Join(img.DOFade(0, 0.3f)));

        return seq.CompletedAsObservable();
    }
}
