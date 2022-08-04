using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class Bresenhammer
{	
	static Vector2 Start(Vector4 half_edge)
	{ return new Vector2(half_edge.x, half_edge.y); }
	
	static Vector2 Middle(Vector4 half_edge)
	{ return new Vector2(half_edge.z, half_edge.w); }
	
	static Vector2 End(Vector4 half_edge)
	{ return Start(half_edge) + Middle(half_edge); }
	
	static Vector2 Minimize(Vector2 a, Vector2 b)
	{ return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y)); }
	
	static Vector2 Maximize(Vector2 a, Vector2 b)
	{ return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y)); }
	
	static Vector2 Minimize(Vector4 half_edge)
	{ return Minimize(Start(half_edge), End(half_edge)); }
	
	static Vector2 Maximize(Vector4 half_edge)
	{ return Maximize(Start(half_edge), End(half_edge)); }
	
	static List<Vector2Int> Bresenham(Vector4 half_edge)
	{
		Vector2 a = Start(half_edge);
		Vector2 b = End(half_edge);
		
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
	
	public static void Draw(Vector4[] half_edges, Vector3Int[] colours, int size, string path)
	{
		if(half_edges.Length == 0){ return; }
		
		// Find extents of vector image
		
		Vector2 r_min = Minimize(half_edges[0]);
		Vector2 r_max = Maximize(half_edges[0]);
		
		for(int i = 1; i < half_edges.Length; i++)
		{
			r_min = Minimize(r_min, Minimize(half_edges[i]));
			r_max = Maximize(r_max, Maximize(half_edges[i]));
		}
		
		// Vector adjustment
		
		Vector2 r_size = r_max - r_min;
		float wh_ratio = r_size.x / r_size.y;
		Vector2Int i_size = new Vector2Int((int) (wh_ratio * size), (size));
		
		for(int i = 0; i < half_edges.Length; i++)
		{
			// Normalize according to extents
			half_edges[i] -= new Vector4(r_min.x, r_min.y, 0, 0);
			half_edges[i] = Vector4.Scale(half_edges[i], new Vector4(1/r_size.x, 1/r_size.y, 1/r_size.x, 1/r_size.y));
			// Scale according to image size
			half_edges[i] = Vector4.Scale(half_edges[i], new Vector4(i_size.x-1, i_size.y-1, i_size.x-1, i_size.y-1));
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
		
		for(int i = 0; i < half_edges.Length; i++)
		{		
			Vector4 half_edge = half_edges[i];
			Vector3Int colour_vec = colours[i];
			System.Drawing.Color colour =
			System.Drawing.Color.FromArgb(colour_vec.x, colour_vec.y, colour_vec.z);
			
			List<Vector2Int> points = Bresenham(half_edge);
			
			foreach(Vector2Int point in points)
			{
				if(point.x < 0 || point.x >= i_size.x){ continue; }
				if(point.y < 0 || point.y >= i_size.y){ continue; }
				
				bitmap.SetPixel(point.x, point.y, colour);
			}
		}
		
		bitmap.Save($"{Application.persistentDataPath}\\{path}.png");
	}
}
