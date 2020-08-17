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
    [SerializeField] Image renderer;

    bool _On = false;
    public bool On
    {
        get => _On;
        set
        {
            _On = value;
            renderer.sprite = _On ? spriteOn : spriteOff;
        }
    }
}
