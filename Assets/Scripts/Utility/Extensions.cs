using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
	public static float Area(this Bounds a)
	{
		return a.size.x * a.size.z;
	}
	
    public static bool Contains(this Bounds a, Bounds b)
	{
		return 
		b.min.x >= a.min.x &&
		b.max.x <= a.max.x &&
		b.min.z >= a.min.z &&
		b.max.z <= a.max.z;
	}
	
	public static Vector4[] Vectorize(this Bounds a)
	{
		Vector4[] half_edges = new Vector4[4];
		half_edges[0] = new Vector4(a.min.x, a.min.z, 0, a.size.z);
		half_edges[1] = new Vector4(a.min.x, a.max.z, a.size.x, 0);
		half_edges[2] = new Vector4(a.max.x, a.max.z, 0, -a.size.z);
		half_edges[3] = new Vector4(a.max.x, a.min.z, -a.size.x, 0);
		return half_edges;
	}
}
