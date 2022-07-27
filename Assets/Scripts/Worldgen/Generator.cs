using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Prefabs")]

    [SerializeField]
    float tile_size;
    [SerializeField]
    GameObject floor_tile;

    [Header("Generation")]

    [SerializeField]
    Texture2D blueprint;
    [SerializeField]
    Vector3 origin;

    Transform floor;

    void Awake()
    {
        floor = new GameObject("Floor").transform;
        floor.position = origin;

        for(int i = 0; i < blueprint.width; i++)
        {
            for(int j = 0; j < blueprint.height; j++)
            {
                if(blueprint.GetPixel(i, j).Equals(Color.red))
                {
                    Transform floor_piece = Instantiate(floor_tile).transform;
                    floor_piece.gameObject.name = $"Floor ({i}, {j})";
                    floor_piece.position = origin + new Vector3(i, 0, j) * tile_size;
                    floor_piece.SetParent(floor);
                }
            }
        }
    }
}
