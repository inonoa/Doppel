using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UnityEngine.UI;
using System.Linq;

public class TransitionView : MonoBehaviour
{
    [SerializeField] Image stairImg;
    [SerializeField] Image bgLImg;
    [SerializeField] Image bgRImg;
    [SerializeField] FloorNumbersView floorNumbersView;
    [SerializeField] FloorNumbersView floorNumbersViewReversed;

    public IObservable<Unit> Enter(int nextFloor)
    {
        Sequence seq = DOTween.Sequence();

        Material stairMat = stairImg.material;
        seq.Append(
            DOTween.To(
                () => stairMat.GetFloat("_Threshold"),
                v => stairMat.SetFloat("_Threshold", v),
                1,
                0.7f
            )
        );

        bgLImg.transform.localPosition = new Vector3(-1920, 0, 0);
        bgRImg.transform.localPosition = new Vector3( 1920, 0, 0);

        seq.AppendInterval(0.2f);

        seq.Append(
            bgLImg.transform
            .DOLocalMoveX(0, 0.8f)
            .SetEase(Ease.OutQuint)
        );
        seq.Join(
            bgRImg.transform
            .DOLocalMoveX(0, 0.8f)
            .SetEase(Ease.OutQuint)
        );

        seq.AppendInterval(0);
        foreach(var numsView in new[] { floorNumbersView, floorNumbersViewReversed })
        {
            numsView.SetNumbers(nextFloor);
            numsView.Images.ForEach(img => img.color = new Color(1, 1, 1, 0));
            numsView.Images.ForEach(img => seq.Join(img.DOFade(1, 0.3f)));
        }

        return seq.CompletedAsObservable();
    }

    public IObservable<Unit> Exit()
    {
        var seq = DOTween.Sequence();

        seq.AppendInterval(0);
        foreach(var numsView in new[] { floorNumbersView, floorNumbersViewReversed })
        {
            numsView.Images.ForEach(img => seq.Join(img.DOFade(0, 0.3f)));
        }

        Material stairMat = stairImg.material;
        seq.Append(
            DOTween.To(
                () => stairMat.GetFloat("_Threshold"),
                v => stairMat.SetFloat("_Threshold", v),
                -0.1f,
                0.7f
            )
        );

        seq.AppendInterval(0.2f);

        seq.Append(
            bgLImg.transform
            .DOLocalMoveY(-1920, 0.8f)
            .SetEase(Ease.InSine)
        );
        seq.Join(
            bgRImg.transform
            .DOLocalMoveY(1920, 0.8f)
            .SetEase(Ease.InSine)
        );

        return seq.CompletedAsObservable();
    }

    void Start()
    {
        bgLImg.transform.localPosition = new Vector3(-1920, 0, 0);
        bgRImg.transform.localPosition = new Vector3( 1920, 0, 0);
        stairImg.material.SetFloat("_Threshold", -0.1f);
        floorNumbersView.BImage.color = new Color(1, 1, 1, 0);
        floorNumbersViewReversed.BImage.color = new Color(1, 1, 1, 0);
    }


    void Update()
    {

    }
}
