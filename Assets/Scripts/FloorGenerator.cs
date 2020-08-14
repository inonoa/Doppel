using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

public class FloorGenerator : SerializedMonoBehaviour
{
    GeneratedMap map;

    [SerializeField] MapGenerator generator;
    [SerializeField] IMapView view;

    void Start()
    {
        map = generator.Generate();
        view.SetMap(map);
        Debug.Log($"主人公: {RandomFloorTile(map)}");
    }

    Vector2Int RandomFloorTile(GeneratedMap map)
    {
        int height = map.Tiles.Count;
        int width = map.Tiles[0].Count;

        Vector2Int ans;
        while(true)
        {
            ans = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
            if(map.Tiles[ans.y][ans.x] == TileType.Floor) break;
        }
        return ans;
    }
}

public interface IMapView
{
    void SetMap(GeneratedMap map);
}
