using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class ParamVec2Int
{
    [SerializeField] [LabelText("0F")] [LabelWidth(50)] [HorizontalGroup("p")] Vector2Int paramOn0F;
    [SerializeField] [LabelText("100F")] [LabelWidth(50)] [HorizontalGroup("p")] Vector2Int paramOn100F;

    public Vector2Int Get(int floor)
    {
        int valX = Mathf.RoundToInt(Mathf.LerpUnclamped(paramOn0F.x, paramOn100F.x, floor / (float)100));
        int valY = Mathf.RoundToInt(Mathf.LerpUnclamped(paramOn0F.y, paramOn100F.y, floor / (float)100));
        return new Vector2Int(valX, valY);
    }
}
