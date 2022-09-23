/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Delaunay
{
	static void SortPoints(Vector2[] points)
	{
		for(int i = 1; i < points.Length; i++)
		{
			float key = points[i].x;
			float aux_key = points[i].y;
			
			for(int j = i-1; j >= 0; j--)
			{
				float comp = points[j].x;
				float aux_comp = points[j].y;
				
				if(comp > key)
				{ ArrTools.Swap(points, j+1, j); }
				else if(comp == key)
				{
					if(aux_comp > aux_key)
					{ ArrTools.Swap(points, j+1, j); }
				}
				else
				{ break; }
			}
		}
	}
	
	static List<Halfedge> Poly(Vector2[] chunk)
	{
		int chunk_size = chunk.Length;
		int poly_size = chunk_size == 3 ? 3 : 1;
		List<Halfedge> poly = new List<Halfedge>();
		
		for(int i = 0; i < poly_size; i++)
		{
			int next = (i+1) % chunk_size;
			poly.Add(Halfedge.FromPoints(chunk[i], chunk[next]));
		}
		
		return poly;
	}
	
	static Vector2 Bottom(List<Halfedge> halfedges)
	{
		Vector2 bottom = halfedges[0].bottom;
		for(int i = 1; i < halfedges.Count; i++)
		{
			Vector2 candidate = halfedges[i].bottom;
			bottom = candidate.y < bottom.y ? candidate : bottom;
		}
		return bottom;
	}
	
	static bool InCircumcircle(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
	{
		// A, B, C MUST BE GIVEN ANTICLOCKWISE
		Matrix4x4 m = default(Matrix4x4);
		m.SetRow(0, new Vector4(a.x, a.y, a.x*a.x + a.y*a.y, 1));
		m.SetRow(1, new Vector4(b.x, b.y, b.x*b.x + b.y*b.y, 1));
		m.SetRow(2, new Vector4(c.x, c.y, c.x*c.x + c.y*c.y, 1));
		m.SetRow(3, new Vector4(d.x, d.y, d.x*d.x + d.y*d.y, 1));
		return m.determinant > 0;
	}
	
	static void SortAngles(Halfedge bottom, List<Halfedge> pool, AngleMode mode)
	{
		for(int i = 1; i < pool.Count; i++)
		{
			Vector2 origin = mode == AngleMode.CLOCKWISE ? bottom.left : bottom.right;
			Vector2 extent = mode == AngleMode.CLOCKWISE ? bottom.right : bottom.left;
			float key = NumTools.Angle(origin, extent, pool[i].top, mode);
			
			for(int j = i-1; j >= 0; j--)
			{
				float comp = NumTools.Angle(origin, extent, pool[j].top, mode);
				
				if(comp > key)
				{ ArrTools.Swap(pool, j+1, j); }
				else
				{ break; }
			}
		}
	}

	static Halfedge PickCandidate(Halfedge bottom, List<Halfedge> pool, AngleMode mode)
	{
		List<Halfedge> cands = new List<Halfedge>();
		foreach(Halfedge halfedge in pool)
		{
			if(halfedge.Contains(bottom.start) || halfedge.Contains(bottom.end))
			{ cands.Add(halfedge); }
		}
		SortAngles(bottom, cands, mode);
		
		Halfedge cand = cands[0];
		if(cands.Count == 1)
		{ return cand; }
		Halfedge aux_cand = cands[1];
		
		Vector2 origin = mode == AngleMode.CLOCKWISE ? bottom.left : bottom.right;
		Vector2 extent = mode == AngleMode.CLOCKWISE ? bottom.right : bottom.left;
		float angle = NumTools.Angle(origin, extent, cand.top, mode);
		
		if(Mathf.Abs(angle) >= 180)
		{ return default(Halfedge); }
		else if(InCircumcircle(bottom.right, cand.top, bottom.left, aux_cand.top))
		{
			pool.Remove(cand);
			return PickCandidate(bottom, pool, mode);
		}
		else
		{ return cand; }
	}
		
	static List<Halfedge> Merge(Vector2[] points, int start, int end)
	{
		int size = end-start;
		
		if(size <= 3)
		{
			Vector2[] chunk = new Vector2[size];
			for(int i = start; i < end; i++)
			{ chunk[i-start] = points[i]; }
			return Poly(chunk);
		}
		else
		{
			int middle = (start+end)/2;
			List<Halfedge> left = Merge(points, start, middle);
			List<Halfedge> right = Merge(points, middle, end);
			List<Halfedge> bottoms = new List<Halfedge>();
			
			Halfedge bottom = Halfedge.FromPoints(Bottom(left), Bottom(right));
			Halfedge l_cand = default(Halfedge);
			Halfedge r_cand = default(Halfedge);
			
			int its = 0;
			
			while(its++ < 300)
			{
				bottoms.Add(bottom);
				
				l_cand = PickCandidate(bottom, left, AngleMode.CLOCKWISE);
				r_cand = PickCandidate(bottom, right, AngleMode.ANTICLOCKWISE);
				Debug.Log($"{l_cand} then {r_cand}");
				
				if(r_cand == default(Halfedge) && l_cand == default(Halfedge))
				{ break; }
				else if(r_cand == default(Halfedge))
				{ bottom = Halfedge.FromPoints(l_cand.top, bottom.end); }
				else if(l_cand == default(Halfedge))
				{ bottom = Halfedge.FromPoints(bottom.start, r_cand.top); }
				else
				{
					if(InCircumcircle(bottom.right, l_cand.top, bottom.left, r_cand.top))
					{ bottom = Halfedge.FromPoints(bottom.start, r_cand.top); }
					else{ bottom = Halfedge.FromPoints(l_cand.top, bottom.end); }
				}
			}
			
			bottoms.AddRange(left);
			bottoms.AddRange(right);
			return bottoms;
		}
	}
	
    public static Mesh Triangulate(Vector3[] points3)
	{
		// Flatten 3D points into XZ plane
		// Sort flattened points
		Vector2[] points = new Vector2[points3.Length];
		for(int i = 0; i < points3.Length; i++)
		{ points[i] = new Vector2(points3[i].x, points3[i].z); }	
		SortPoints(points);

		// Chunk points into segments and triangles
		Halfedge[] mesh = Merge(points, 0, points.Length).ToArray();
		Vector3Int[] colours = ArrTools.Repeat(new Vector3Int(255, 0, 0), mesh.Length);
		Bresenhammer.Draw(mesh, colours, 512, "mesh");
	
		return null;
	}
}*/
