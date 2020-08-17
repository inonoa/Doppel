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
    [SerializeField] Color doppelColor = new Color(0.5f, 0, 1, 1);

    FloorStatus status;

    public void SetStatus(FloorStatus status)
    {
        this.status = status;
    }
    public void SetActive(bool active) => gameObject.SetActive(active);

    enum TileStatus{ Wall, Aisle, RoomFloor, Stair, Hero, HeroView, Doppel }

    TileStatus ToStatus(TileType type)
    {
        switch(type)
        {
        case TileType.Wall: return TileStatus.Wall;
        case TileType.Aisle: return TileStatus.Aisle;
        case TileType.RoomFloor: return TileStatus.RoomFloor;
        case TileType.Stair: return TileStatus.Stair;
        default: return TileStatus.Wall;
        }
    }

    string ToDebugStr(TileStatus tile)
    {
        switch (tile)
        {
        case TileStatus.Aisle:     return "_ ";
        case TileStatus.RoomFloor: return "  ";
        case TileStatus.Stair:     return "L ";
        case TileStatus.Wall:      return "W ";

        case TileStatus.Hero:     return Colored("P ", heroColor);
        case TileStatus.HeroView: return Colored("X ", heroColor);
        case TileStatus.Doppel:   return Colored("D ", doppelColor);

        default: return Colored("XX", Color.red);
        }
    }

    string Colored(string str, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";
    }

    TileStatus[][] ToTiles(FloorStatus status)
    {
        TileStatus[][] ans = status.map.Tiles.Select(row => 
                             row.Select(tile => ToStatus(tile)).ToArray()
                             ).ToArray();
        ans[status.hero.PosOnMap.y][status.hero.PosOnMap.x] = TileStatus.Hero;
        foreach(Vector2Int view in status.hero.GetView())
        {
            ans[view.y][view.x] = TileStatus.HeroView;
        }
        status.doppels.ForEach(dp => {
            ans[dp.PosOnMap.y][dp.PosOnMap.x] = TileStatus.Doppel;
        });
        return ans;
    }

    void Update()
    {
        var tiles = ToTiles(status);

        StringBuilder mapStr = new StringBuilder("  ");

        for(int i = 0; i < tiles[0].Length; i++)
        {
            mapStr.Append(i.ToString("00"));
        }
        mapStr.Append("\n");

        for(int i = 0; i < tiles.Length; i++)
        {
            mapStr.Append(i.ToString("00"));
            for(int j = 0; j < tiles[i].Length; j++)
            {
                mapStr.Append(ToDebugStr(tiles[i][j]));
            }
            mapStr.Append("\n");
        }

        text.text = mapStr.ToString();
    }
}
