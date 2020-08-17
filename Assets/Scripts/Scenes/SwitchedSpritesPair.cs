using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SwitchedSpritesPair
{
    [SerializeField] Sprite spriteOn;
    [SerializeField] Sprite spriteOff;
    [SerializeField] Image _Renderer;

    public Image Renderer => _Renderer;

    bool _On = false;
    public bool On
    {
        get => _On;
        set
        {
            _On = value;
            _Renderer.sprite = _On ? spriteOn : spriteOff;
        }
    }
}
