using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Worldgen/Genset")]
public class Genset : ScriptableObject
{
    [Header("Settings")]

    [SerializeField]
    float tile_width;
    [SerializeField]
    float tile_scale;

    public float tile_size => tile_width * tile_scale;

    [Header("Prefabs")]

    [SerializeField]
    GameObject room_0;
    [SerializeField]
    GameObject niche_1;
    [SerializeField]
    GameObject corner_3;
    [SerializeField]
    GameObject hall_6;
    [SerializeField]
    GameObject edge_7;
    [SerializeField]
    GameObject floor_15;

    GameObject[] tiles;

    public void Prime()
    {
        foreach(GameObject tile in tiles){ Destroy(tile); }

        tiles = new GameObject[]
        {
            Instantiate(room_0), // 0
            Instantiate(niche_1), // 1
            Instantiate(niche_1, Vector3.zero, Quaternion.Euler(0, 270, 0)), // 2
            Instantiate(corner_3), // 3
            Instantiate(niche_1, Vector3.zero, Quaternion.Euler(0, 90, 0)), // 4
            Instantiate(corner_3, Vector3.zero, Quaternion.Euler(0, 90, 0)), // 5
            Instantiate(hall_6), // 6
            Instantiate(edge_7), // 7
            Instantiate(niche_1, Vector3.zero, Quaternion.Euler(0, 180, 0)), // 8
            Instantiate(hall_6, Vector3.zero, Quaternion.Euler(0, 270, 0)), // 9
            Instantiate(corner_3, Vector3.zero, Quaternion.Euler(0, 270, 0)), // 10
            Instantiate(edge_7, Vector3.zero, Quaternion.Euler(0, 270, 0)), // 11
            Instantiate(corner_3, Vector3.zero, Quaternion.Euler(0, 180, 0)), // 12
            Instantiate(edge_7, Vector3.zero, Quaternion.Euler(0, 90, 0)), // 13
            Instantiate(edge_7, Vector3.zero, Quaternion.Euler(0, 180, 0)), // 14
            Instantiate(floor_15) // 15
        };

        foreach(GameObject tile in tiles){ tile.transform.localScale = Vector3.one * tile_scale; }
        foreach(GameObject tile in tiles){ tile.SetActive(false); }
    }

    public GameObject BitmaskTile(int bits)
    {
        GameObject tile = Instantiate(tiles[bits]);
        tile.SetActive(true);
        return tile;
    }
}
