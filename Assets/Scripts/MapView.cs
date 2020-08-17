using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapView : MonoBehaviour, IMapView
{
    [SerializeField] MeshFilter wallPrefab;
    [SerializeField] MeshFilter floorPrefab;
    [SerializeField] float floorY = -0.5f;
    [SerializeField] float _TileSize = 1;
    public float TileSize => _TileSize;
    [SerializeField] Color herVviewColor = new Color(0.2f, 0.2f, 0.2f, 1);

    FloorStatus status;

    MeshRenderer[][] floorRenderers;

    public void SetStatus(FloorStatus status)
    {
        this.status = status;
        floorRenderers = new MeshRenderer[status.map.Height][];

        Vector3 upPosition = new Vector3(0, 0, (status.map.Height - 1) / 2.0f * _TileSize);
        for(int i = 0; i < status.map.Height; i ++)
        {
            GameObject row = new GameObject("Row" + i);
            row.transform.SetParent(this.transform);
            row.transform.position = upPosition - i * new Vector3(0, 0, _TileSize);

            floorRenderers[i] = new MeshRenderer[status.map.Width];

            Vector3 leftPosition = new Vector3(- (status.map.Width - 1) / 2.0f * _TileSize, 0, 0);
            for(int j = 0; j < status.map.Width; j ++)
            {
                if(status.map.Tiles[i][j] == TileType.Wall)
                {
                    MeshFilter wall = Instantiate(wallPrefab, row.transform);
                    wall.transform.localPosition = leftPosition + j * new Vector3(_TileSize, 0, 0);
                }

                MeshFilter floor = Instantiate(floorPrefab, row.transform);
                floor.transform.localPosition = new Vector3(0, floorY, 0) + leftPosition + j * new Vector3(_TileSize, 0, 0);
                floorRenderers[i][j] = floor.GetComponent<MeshRenderer>();
            }
        }
    }

    public void SetActive(bool active) => gameObject.SetActive(active);

    void Update()
    {
        Vector2Int[] view = status.hero.GetView()
                            .Append(status.hero.PosOnMap)
                            .ToArray();
        foreach(int i in Enumerable.Range(0, floorRenderers.Length))
        {
            foreach(int j in Enumerable.Range(0, floorRenderers[i].Length))
            {
                if(view.Contains(new Vector2Int(j, i)))
                {
                    floorRenderers[i][j].material.color = herVviewColor;
                }
                else
                {
                    floorRenderers[i][j].material.color = new Color(1, 1, 1, 1);
                }
            }
        }
    }
}
