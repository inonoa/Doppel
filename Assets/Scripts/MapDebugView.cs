using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;

public class MapDebugView : MonoBehaviour, IMapView
{
    [SerializeField] Text text;

    public void SetMap(GeneratedMap map)
    {
        string txt = "";

        for(int i = 0; i < map.Tiles[0].Count; i++)
        {
            txt += i.ToString("00");
        }
        txt += "\n";

        for(int i = 0; i < map.Tiles.Count; i++)
        {
            txt += i.ToString("00");
            foreach(TileType tile in map.Tiles[i])
            {
                txt += tile.ToDebugChar() + " ";
            }
            txt += "\n";
        }

        text.text = txt;
    }
}
