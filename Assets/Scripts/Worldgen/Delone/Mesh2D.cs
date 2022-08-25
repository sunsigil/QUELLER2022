using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Edge
{
	public Vector2 a;
	public Vector2 b;

	public static bool operator ==(Edge e1, Edge e2)
	{
		return
		(e1.a == e2.a && e1.b == e2.b) ||
		(e1.a == e2.b && e1.b == e2.a);
	}
	public static bool operator !=(Edge e1, Edge e2)
	{ return !(e1 == e2); }
    public override bool Equals(object obj)
    { return (Edge)obj == this; }

    public Edge(Vector2 a, Vector2 b)
    {
		this.a = a;
		this.b = b;
    }

	public Vector2 Midpoint()
    { return (a + b) * 0.5f; }
	
	public float Slope()
    { return (b.y - a.y) / (b.x - a.x); }
}

public struct Triangle
{
	public Vector2 a;
	public Vector2 b;
	public Vector2 c;

	public Edge ab;
	public Edge bc;
	public Edge ca;

    public static bool operator ==(Triangle t1, Triangle t2)
    {
		return
		t1.a == t2.a &&
		t1.b == t2.b &&
		t1.c == t2.c;
	}
	public static bool operator !=(Triangle t1, Triangle t2)
	{ return !(t1 == t2); }
	public override bool Equals(object obj)
	{ return (Triangle)obj == this; }

	public Triangle(Vector2 a, Vector2 b, Vector2 c)
    {
		Vector2[] set = new Vector2[] { a, b, c };
		// DeloneMath.ACWSort(set);

		this.a = set[0];
		this.b = set[1];
		this.c = set[2];

		ab = new Edge(this.a, this.b);
		bc = new Edge(this.b, this.c);
		ca = new Edge(this.c, this.a);
    }

	public bool HasVertex(Vector2 v)
    {
		return
		a == v ||
		b == v ||
		c == v;
    }

	public Vector2 Circumcenter()
	{
		Vector2 M_ab = ab.Midpoint();
		Vector2 M_bc = bc.Midpoint();

		float m_ab = ab.Slope();
		float m_bc = bc.Slope();

		float n_ab = -(1 / m_ab);
		float n_bc = -(1 / m_bc);

		// y - k = n(x - h)
		// n(x - h) - y + k = 0
		// nx - nh - y + k = 0
		// (n)x + (-1)y = (nh - k)
		MatrixMxN system = new MatrixMxN(2, 3);
		system.SetRow(0, new float[] { n_ab, -1, n_ab * M_ab.x - M_ab.y });
		system.SetRow(1, new float[] { n_bc, -1, n_bc * M_bc.x - M_bc.y });
		MatrixMxN rref = MatrixMxN.RREF(system);

		return new Vector2(rref.Get(0, 2), rref.Get(1, 2));
	}

	public float Circumradius()
	{
		Vector2 center = Circumcenter();
		return Vector2.Distance(center, a);
	}

	public Halfedge[] Vectorize()
    {
		Halfedge[] halfedges = new Halfedge[3];
		halfedges[0] = Halfedge.FromPoints(a, b);
		halfedges[1] = Halfedge.FromPoints(b, c);
		halfedges[2] = Halfedge.FromPoints(c, a);
		return halfedges;
    }
}

public struct Circle
{
	public Vector2 center;
	public float radius;

	public Circle(Vector2 center, float radius)
    {
		this.center = center;
		this.radius = radius;
    }

	public bool Contains(Vector2 v)
    { return Vector2.Distance(center, v) <= radius; }
}

public class Mesh2D
{
	List<Vector2> vertices;
	List<Triangle> triangles;

	public Triangle Supertriangle()
	{
		Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
		Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

		foreach (Vector2 v in vertices)
		{
			min.x = Mathf.Min(min.x, v.x);
			min.y = Mathf.Min(min.y, v.y);
			max.x = Mathf.Max(max.x, v.x);
			max.y = Mathf.Max(max.y, v.y);
		}

		Vector2 span = (max - min) * 10;
		Vector2 a = new Vector2(-200, -100); // new Vector2(min.x - span.x, min.y - span.y * 3);
		Vector2 b = new Vector2(300, -200); // new Vector2(min.x - span.x, max.y + span.y);
		Vector2 c = new Vector2(100, 400); // new Vector2(max.x + span.x * 3, max.y + span.y);

		return new Triangle(a, b, c);
	}

	public Halfedge[] Vectorize()
    {
		List<Halfedge> list = new List<Halfedge>();

		foreach(Triangle t in triangles)
        {
			list.Add(Halfedge.FromPoints(t.a, t.b));
			list.Add(Halfedge.FromPoints(t.b, t.c));
			list.Add(Halfedge.FromPoints(t.c, t.a));
		}

		return list.ToArray();
    }

	List<Edge> UniqueEdges(List<Edge> edges)
    {
		List<Edge> unique_edges = new List<Edge>();

		for (int i = 0; i < edges.Count; i++)
		{
			bool unique = true;

			for (int j = 0; j < edges.Count; j++)
			{
				if (j == i)
				{ continue; }

				if (edges[j] == edges[i])
				{
					unique = false;
					break;
				}
			}

			if (unique)
			{ unique_edges.Add(edges[i]); }
		}

		return unique_edges;
	}

	public Mesh2D(Vector3[] positions)
	{
		vertices = new List<Vector2>();
		triangles = new List<Triangle>();

		foreach(Vector3 position in positions)
        { vertices.Add(new Vector2(position.x, position.z)); }
		DeloneMath.DeloneSort(vertices);

		Triangle supertriangle = Supertriangle();
		triangles.Add(supertriangle);
		
		foreach(Vector2 v in vertices)
        {
			List<Triangle> bad_triangles = new List<Triangle>();		
			foreach(Triangle t in triangles)
            {
				Circle circ = new Circle(t.Circumcenter(), t.Circumradius());
				if(circ.Contains(v))
				{ bad_triangles.Add(t); }
            }

			List<Edge> bad_edges = new List<Edge>();
			foreach (Triangle t in bad_triangles)
			{
				bad_edges.Add(t.ab);
				bad_edges.Add(t.bc);
				bad_edges.Add(t.ca);
			}

			List<Edge> hole = UniqueEdges(bad_edges);

			foreach(Triangle t in bad_triangles)
            { triangles.Remove(t); }

			foreach(Edge e in hole)
            {
				Triangle t = new Triangle(e.a, e.b, v);
				triangles.Add(t);
            }
        }

		for(int i = 0; i < triangles.Count; i++)
        {
			Triangle t = triangles[i];

			if(
				t.HasVertex(supertriangle.a) ||
				t.HasVertex(supertriangle.b) ||
				t.HasVertex(supertriangle.c))
			{
				triangles.RemoveAt(i);
				i--;
			}
        }
	}
}