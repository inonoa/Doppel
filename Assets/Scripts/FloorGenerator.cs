using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

public class FloorGenerator : SerializedMonoBehaviour
{

    [SerializeField] MapGenerator generator;
    [SerializeField] IMapView view;
    [SerializeField] HeroMover heroPrefab;
    [SerializeField] DoppelMover doppelPrefab;

    public FloorStatus Generate()
    {
        var map = generator.Generate();
        Debug.Log($"主人公: {RandomFloorTile(map)}");
        var hero = Instantiate(heroPrefab);
        List<DoppelMover> doppels = new List<DoppelMover>();
        for(int i = 0; i < 5; i ++){
            DoppelMover doppel = Instantiate(doppelPrefab);
            doppel.Init(hero);
            doppels.Add(doppel);
        }
        FloorStatus status = new FloorStatus(map, hero, doppels);
        view.SetStatus(status);

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

public interface IMapView
{
    void SetStatus(FloorStatus status);
}
