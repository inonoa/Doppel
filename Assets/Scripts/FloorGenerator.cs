using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

public class FloorGenerator : SerializedMonoBehaviour
{

    [SerializeField] MapGenerator generator;
    [SerializeField] HeroMover heroPrefab;
    [SerializeField] DoppelMover doppelPrefab;
    [SerializeField] ParamInt initialNumDoppels;
    public FloorStatus Generate(int floor, ViewParams viewParams)
    {
        var map = generator.Generate(floor);
        var hero = Instantiate(heroPrefab, transform);
        List<DoppelMover> doppels = new List<DoppelMover>();
        FloorStatus status = new FloorStatus(map, hero, doppels);
        hero.Init(RandomFloorTile(map), status, viewParams);
        for(int i = 0; i < initialNumDoppels.Get(floor); i ++){
            DoppelMover doppel = Instantiate(doppelPrefab, transform);
            doppel.Init(status, RandomFloorTile(map), viewParams);
            doppels.Add(doppel);
        }

        return status;
    }

    Vector2Int RandomFloorTile(GeneratedMap map)
    {
        int height = map.Tiles.Count;
        int width = map.Tiles[0].Count;

        Vector2Int ans;
        while(true)
        {
            ans = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
            if(map.Tiles[ans.y][ans.x] == TileType.RoomFloor) break;
        }
        return ans;
    }
}

