using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour, IMapView
{
    [SerializeField] MeshFilter wallPrefab;
    [SerializeField] MeshFilter floorPrefab;
    [SerializeField] float floorY = -0.5f;
    [SerializeField] float _TileSize = 1;
    public float TileSize => _TileSize;

    FloorStatus status;

    public void SetStatus(FloorStatus status)
    {
        this.status = status;

        Vector3 upPosition = new Vector3(0, 0, (status.map.Height - 1) / 2.0f * _TileSize);
        for(int i = 0; i < status.map.Height; i ++)
        {
            GameObject row = new GameObject("Row" + i);
            row.transform.SetParent(this.transform);
            row.transform.position = upPosition - i * new Vector3(0, 0, _TileSize);

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
            }
        }
    }
}
