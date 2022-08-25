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
	
	public static Halfedge[] Vectorize(this Bounds a)
	{
		Halfedge[] halfedges = new Halfedge[4];
		halfedges[0] = new Halfedge(a.min.x, a.min.z, 0, a.size.z);
		halfedges[1] = new Halfedge(a.min.x, a.max.z, a.size.x, 0);
		halfedges[2] = new Halfedge(a.max.x, a.max.z, 0, -a.size.z);
		halfedges[3] = new Halfedge(a.max.x, a.min.z, -a.size.x, 0);
		return halfedges;
	}
}
