using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
	[SerializeField]
	bool gizmoid;
	
    [SerializeField]
    Texture2D blueprint;
    [SerializeField]
    Genset genset;
    [SerializeField]
    Vector3 origin;

    Transform town;

    bool CheckPixel(int x, int y)
    { return blueprint.GetPixel(x, y).Equals(Color.red); }

    void Awake()
    {
        genset.Prime();

        town = new GameObject("Town").transform;
        town.position = origin;

        for(int i = 1; i < blueprint.width-1; i++)
        {
            for(int j = 1; j < blueprint.height-1; j++)
            {
                if(!CheckPixel(i, j)){ continue; }

                int bits = 0;
                if(CheckPixel(i, j+1)){ bits |= 8; } // N
                if(CheckPixel(i-1, j)){ bits |= 4; } // W
                if(CheckPixel(i+1, j)){ bits |= 2; } // E
                if(CheckPixel(i, j-1)){ bits |= 1; } // S

                Transform piece = genset.BitmaskTile(bits).transform;
                piece.gameObject.name = $"Town ({i}, {j}) [{System.Convert.ToString(bits, 2)}]";
                piece.position = origin + new Vector3(i, 0, j) * genset.tile_size;
                piece.SetParent(town);
            }
        }

		PathNode[] mesh_targets = FindObjectsOfType<PathNode>();
		Vector3[] positions = new Vector3[mesh_targets.Length];
		for(int i = 0; i < positions.Length; i++)
		{ positions[i] = mesh_targets[i].transform.position; }

		Mesh2D mesh = new Mesh2D(positions);
		Bresenhammer.Draw(mesh.Vectorize(), 2048, "mesh");
    }
	
	void OnDrawGizmos()
	{
		if(!gizmoid){ return; }
		
		if(blueprint != null && genset != null)
		{
			Color color = Color.white;
			color.a = 0.25f;
			Gizmos.color = color;
			
			for(int i = 1; i < blueprint.width-1; i++)
			{
				for(int j = 1; j < blueprint.height-1; j++)
				{
					if(!CheckPixel(i, j)){ continue; }
						
					Vector3 pos = origin + Vector3.up * 0.5f;
					pos += new Vector3(i, 0, j);
					pos *= genset.tile_size;
					
					Gizmos.DrawWireCube(pos, Vector3.one * genset.tile_size);
				}
			}
		}
	}
}
