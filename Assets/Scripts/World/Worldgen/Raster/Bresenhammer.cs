using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class Bresenhammer
{		
	static List<Vector2Int> Bresenham(Halfedge halfedge)
	{
		Vector2 a = halfedge.start;
		Vector2 b = halfedge.end;
		
		int x0 = (int) Mathf.Round(a.x);
		int y0 = (int) Mathf.Round(a.y);
		int x1 = (int) Mathf.Round(b.x);
		int y1 = (int) Mathf.Round(b.y);
		
		int dx = Mathf.Abs(x1 - x0);
		int sx = x0 < x1 ? 1 : -1;
		int dy = -Mathf.Abs(y1 - y0);
		int sy = y0 < y1 ? 1 : -1;
		int e = dx + dy;
		
		int x = x0;
		int y = y0;	
		List<Vector2Int> points = new List<Vector2Int>();
	
		while(true)
		{
			points.Add(new Vector2Int(x, y));
			
			if(x == x1 && y == y1){ break; }
			
			int e2 = 2 * e;
			
			if(e2 >= dy)
			{
				if(x == x1){ break; }
				
				e += dy;
				x += sx;
			}
			if(e2 <= dx)
			{
				if(y == y1){ break; }
				
				e += dx;
				y += sy;
			}
		}
		
		return points;
	}
	
	public static void Draw(Halfedge[] halfedges, int size, string path, Vector3Int[] colours = null)
	{
		if(halfedges.Length == 0){ return; }

		if(colours == null)
		{ colours = ArrTools.Repeat(new Vector3Int(255, 0, 0), halfedges.Length); }
		
		// Find extents of vector image
		
		Vector2 r_min = halfedges[0].min;
		Vector2 r_max = halfedges[0].max;
		
		for(int i = 1; i < halfedges.Length; i++)
		{
			Halfedge cand = halfedges[i];
			
			float min_x = Mathf.Min(r_min.x, cand.min.x);
			float min_y = Mathf.Min(r_min.y, cand.min.y);
			r_min = new Vector2(min_x, min_y);
			
			float max_x = Mathf.Max(r_max.x, cand.max.x);
			float max_y = Mathf.Max(r_max.y, cand.max.y);
			r_max = new Vector2(max_x, max_y);
		}
		
		// Vector adjustment
		
		Vector2 r_size = r_max - r_min;
		float wh_ratio = r_size.x / r_size.y;
		Vector2Int i_size = new Vector2Int((int) (wh_ratio * size), (size));
		
		for(int i = 0; i < halfedges.Length; i++)
		{
			// Normalize according to extents
			halfedges[i] = halfedges[i].ShiftStart(-r_min);
			halfedges[i] = halfedges[i].ScaleBy(new Vector2(1/r_size.x, 1/r_size.y));
			// Scale according to image size
			halfedges[i] = halfedges[i].ScaleBy(new Vector2(i_size.x-1, i_size.y-1));
		}
		
		// Construct bitmap
		
		Bitmap bitmap = new Bitmap(i_size.x, i_size.y);
		
		for(int x = 0; x < i_size.x; x++)
		{
			for(int y = 0; y < i_size.y; y++)
			{
				bitmap.SetPixel(x, y, System.Drawing.Color.Black);
			}
		}
		
		for(int i = 0; i < halfedges.Length; i++)
		{		
			Halfedge halfedge = halfedges[i];
			Vector3Int colour_vec = colours[i];
			System.Drawing.Color colour =
			System.Drawing.Color.FromArgb(colour_vec.x, colour_vec.y, colour_vec.z);
			
			List<Vector2Int> points = Bresenham(halfedge);
			
			foreach(Vector2Int point in points)
			{
				if(point.x < 0 || point.x >= i_size.x){ continue; }
				if(point.y < 0 || point.y >= i_size.y){ continue; }
				
				bitmap.SetPixel(point.x, i_size.y-1-point.y, colour);
			}
		}
		
		bitmap.Save($"{Application.persistentDataPath}\\{path}.png");
	}
}
