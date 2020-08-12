using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;

public class MapView : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] MapGenerator generator;

    void Start()
    {
        GenerateFromView();
    }

    [Button]
    void GenerateFromView()
    {
        string txt = "";
        IReadOnlyList<IReadOnlyList<TileType>> map = generator.Generate().Tiles;

        foreach(IReadOnlyList<TileType> row in map)
        {
            foreach(TileType tile in row)
            {
                txt += tile.ToDebugChar() + " ";
            }
            txt += "\n";
        }

        text.text = txt;
    }


    void Update()
    {
        
    }
}
