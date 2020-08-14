using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;
using System.Text;

public class MapDebugView : MonoBehaviour, IMapView
{
    [SerializeField] Text text;
    [SerializeField] Color heroColor = new Color(1, 0, 0, 1);

    FloorStatus status;

    public void SetStatus(FloorStatus status)
    {
        this.status = status;
    }

    void Update()
    {
        StringBuilder mapStr = new StringBuilder("  ");

        for(int i = 0; i < status.map.Tiles[0].Count; i++)
        {
            mapStr.Append(i.ToString("00"));
        }
        mapStr.Append("\n");

        for(int i = 0; i < status.map.Tiles.Count; i++)
        {
            mapStr.Append(i.ToString("00"));
            for(int j = 0; j < status.map.Tiles[i].Count; j++)
            {
                if(status.hero.PosOnMap == new Vector2Int(j, i))
                {
                    mapStr.Append($"<color=#{ColorUtility.ToHtmlStringRGB(heroColor)}>P </color>");
                }
                else
                {
                    mapStr.Append(status.map.Tiles[i][j].ToDebugChar() + " ");
                }
            }
            mapStr.Append("\n");
        }

        text.text = mapStr.ToString();
    }
}
