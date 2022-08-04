using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTreeGizmo : MonoBehaviour
{
	[SerializeField]
	bool gizmoid;
	
	RTree<PathCell> tree;
	PathCell[] cells;
	
	void Start()
	{
		tree = new RTree<PathCell>();
		cells = FindObjectsOfType<PathCell>();
		foreach(PathCell cell in cells)
		{ tree.Insert(cell); }
		
		List<RNode> elements = tree.Elements();
		List<PathCell> entries = tree.Entries();
		int el = elements.Count;
		int en = entries.Count;
		int i = 0;
		
		Bounds[] mbrs = new Bounds[el + en];
		for(; i < en; i++)
		{ mbrs[i] = entries[i].MBR(); }
		int offset = i;
		for(; i < offset + el; i++)
		{ mbrs[i] = elements[i-offset].MBR(); }
		
		Vector4[] images = new Vector4[mbrs.Length * 4];
		Vector3Int[] colours = new Vector3Int[images.Length];
		
		for(i = 0; i < mbrs.Length; i++)
		{
			Vector4[] image = mbrs[i].Vectorize();
			for(int j = 0; j < 4; j++)
			{
				images[i * 4 + j] = image[j];
				colours[i * 4 + j] = i < en ? new Vector3Int(0, 0, 0) : new Vector3Int(255, 0, 0);
			}
		}
		
		Bresenhammer.Draw(images, colours, 512, "rtree");
	}
	
	void OnDrawGizmos()
	{
		if(!gizmoid){ return; }
		
		if(tree != null)
		{
			List<RNode> elements = tree.Elements();
			List<PathCell> entries = tree.Entries();
			int el = elements.Count;
			int en = entries.Count;
			int i = 0;
			
			Bounds[] mbrs = new Bounds[el + en];
			for(; i < en; i++)
			{ mbrs[i] = entries[i].MBR(); }
			int offset = i;
			for(; i < offset + el; i++)
			{ mbrs[i] = elements[i-offset].MBR(); }
		
			for(i = 0; i < mbrs.Length; i++)
			{
				Gizmos.color = i < en ? Color.green : Color.red;
				Gizmos.DrawWireCube(mbrs[i].center, mbrs[i].size);
			}
		}
	}
}