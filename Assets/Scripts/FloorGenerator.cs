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

    public FloorStatus Generate()
    {
        var map = generator.Generate();
        var hero = Instantiate(heroPrefab);
        List<DoppelMover> doppels = new List<DoppelMover>();
        FloorStatus status = new FloorStatus(map, hero, doppels);
        hero.Init(RandomFloorTile(map), status);
        for(int i = 0; i < 5; i ++){
            DoppelMover doppel = Instantiate(doppelPrefab);
            doppel.Init(status, RandomFloorTile(map));
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
            if(map.Tiles[ans.y][ans.x] == TileType.Floor) break;
        }
        return ans;
    }
}

