using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNode : IBoundable
{
	List<IBoundable> _children;
	public List<IBoundable> children => _children;

	Bounds _mbr;
	public Bounds MBR(){ return _mbr; }

	bool _is_leaf;
	public bool is_leaf => _is_leaf;

	public static RNode MinimizeFullness(RNode a, RNode b)
	{ return a.children.Count > b.children.Count ? b : a; }

	public static RNode MinimizeArea(RNode a, RNode b)
	{
		float a_area = a.MBR().Area();
		float b_area = b.MBR().Area();

		if(a_area == b_area)
		{ return MinimizeFullness(a, b); }
		return a_area > b_area ? b : a;
	}

	public static RNode MinimizeGrowth(RNode a, RNode b, IBoundable incl)
	{
		Bounds a_incl = a.MBR();
		a_incl.Encapsulate(incl.MBR());
		Bounds b_incl = b.MBR();
		b_incl.Encapsulate(incl.MBR());

		float a_growth = a_incl.Area() - a.MBR().Area();
		float b_growth = b_incl.Area() - b.MBR().Area();

		if(a_growth == b_growth)
		{ return MinimizeArea(a, b); }
		return a_growth > b_growth ? b : a;
	}

	public void RecalculateMBR()
	{
		if(_children.Count == 0){ _mbr = new Bounds(Vector3.zero, Vector3.zero); }

		Vector3 min = _children[0].MBR().min;
		Vector3 max = _children[0].MBR().max;

		for(int i = 1; i < _children.Count; i++)
		{
			Bounds bound = _children[i].MBR();
			min.x = Mathf.Min(min.x, bound.min.x);
			min.z = Mathf.Min(min.z, bound.min.z);
			max.x = Mathf.Max(max.x, bound.max.x);
			max.z = Mathf.Max(max.z, bound.max.z);
		}

		_mbr = new Bounds((min+max)*0.5f, max-min);
	}

	public RNode(List<IBoundable> children, bool is_leaf)
	{
		_children = children == null ? new List<IBoundable>() : children;
		_is_leaf = is_leaf;
	}
}
