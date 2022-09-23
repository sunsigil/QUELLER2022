using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeloneMath
{
	public static Vector2 Leftmost(Vector2 a, Vector2 b)
	{ return b.x < a.x ? b : a; }
	public static Vector2 Rightmost(Vector2 a, Vector2 b)
	{ return b.x > a.x ? b : a; }
	public static Vector2 Upmost(Vector2 a, Vector2 b)
	{ return b.y > a.y ? b : a; }
	public static Vector2 Downmost(Vector2 a, Vector2 b)
	{ return b.y < a.y ? b : a; }

	static int DeloneOrderCompare(Vector2 a, Vector2 b)
	{
		if (a.x != b.x)
		{ return (int)(a.x - b.x); }
		else
		{ return (int)(a.y - b.y); }
	}

	public static void DeloneSort(List<Vector2> vertices)
    { vertices.Sort(DeloneOrderCompare); }

	static float ACWAngleDistance(Vector2 fulcrum, Vector2 point)
    { return Vector2.SignedAngle(point, fulcrum); }

	public static void ACWSort(Vector2[] vertices)
    {
		Vector2 centroid = Vector2.zero;
		for(int i = 0; i < vertices.Length; i++)
        { centroid += vertices[i]; }
		centroid /= vertices.Length;

		FulcrumComparer<Vector2, float> comparer = new FulcrumComparer<Vector2, float>(centroid, ACWAngleDistance);
		 
		Array.Sort(vertices, comparer);
	}
}
