using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class FloorNumbersView : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Image numImagePrefab;
    [SerializeField] Image _BImage;
    public Image BImage => _BImage;
    public IReadOnlyList<Image> Images => numImages.Append(BImage).ToList();
    [SerializeField] bool isReversed;

    readonly float digitWidth = 6 * 1920 / (float)80;

    List<Image> numImages = new List<Image>();

    void Start()
    {
        
    }


    public void SetNumbers(int num)
    {
        numImages.ForEach(img => Destroy(img.gameObject));
        numImages.Clear();

        string numStr = num.ToString();
        foreach(int i in Enumerable.Range(0, numStr.Length))
        {
            int digitNum = int.Parse(numStr.Substring(i, 1));
            Image numImage = Instantiate(numImagePrefab, this.transform);
            numImage.transform.localPosition = new Vector3(digitWidth, 0, 0) * i * (isReversed ? -1 : 1);
            numImage.sprite = numberSprites[digitNum];
            numImages.Add(numImage);
        }
    }
}
