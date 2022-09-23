using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RTreeCapture : MonoBehaviour
{
	static RTree<PathNode> tree;
	static PathNode[] cells;
	
	Bounds[] mbrs;
	Halfedge[] images;
	Vector3Int[] colours;

	void Start()
	{
		tree = new RTree<PathNode>();
		cells = FindObjectsOfType<PathNode>();
		
		foreach(PathNode cell in cells)
		{ tree.Insert(cell); }

		List<RNode> elements = tree.Elements();
		List<PathNode> entries = tree.Entries();
		int el = elements.Count;
		int en = entries.Count;
		int i = 0;

		mbrs = new Bounds[el + en];
		for(; i < en; i++)
		{ mbrs[i] = entries[i].mbr; }
		int offset = i;
		for(; i < offset + el; i++)
		{ mbrs[i] = elements[i-offset].mbr; }
		
		images = new Halfedge[mbrs.Length * 4];
		colours = new Vector3Int[images.Length];
		for(i = 0; i < mbrs.Length; i++)
		{
			Halfedge[] image = mbrs[i].Vectorize();
			for(int j = 0; j < 4; j++)
			{
				images[i * 4 + j] = image[j];
				colours[i * 4 + j] = i < en ? new Vector3Int(0, 0, 0) : new Vector3Int(255, 0, 0);
			}
		}

		Bresenhammer.Draw(images, 1024, $"rtree_h{tree.height}_o{tree.overload}", colours);
	}
}
