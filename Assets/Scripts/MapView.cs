using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MapView : MonoBehaviour
{
    [SerializeField] Text text;

    void Start()
    {
        string txt = "";
        IReadOnlyList<IReadOnlyList<TileType>> map = new MapGenerator().Generate().Tiles;

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
