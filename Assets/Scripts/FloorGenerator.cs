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
    HeroMover currentHero;

    void Start()
    {
        var map = generator.Generate();
        Debug.Log($"主人公: {RandomFloorTile(map)}");
        currentHero = Instantiate(heroPrefab);
        view.SetStatus(new FloorStatus(map, currentHero));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) currentHero.Move(HeroMover.Direction.R);
        else if(Input.GetKeyDown(KeyCode.LeftArrow)) currentHero.Move(HeroMover.Direction.L);
        else if(Input.GetKeyDown(KeyCode.UpArrow)) currentHero.Move(HeroMover.Direction.U);
        else if(Input.GetKeyDown(KeyCode.DownArrow)) currentHero.Move(HeroMover.Direction.D);
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
