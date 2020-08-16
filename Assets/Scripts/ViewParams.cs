using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ViewParams
{
    [SerializeField] float _TileSize;
    public float TileSize => _TileSize;
}
