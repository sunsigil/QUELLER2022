using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNode : IBoundable
{
	List<IBoundable> _children;
	public List<IBoundable> children => _children;
	public int fill => _children.Count;

	Bounds _mbr;
	public Bounds mbr => _mbr;
	
	int _level;
	public int level => _level;
	public bool is_leaf => _level == 0;

	public static RNode MinimizeFullness(RNode a, RNode b)
	{ return a.children.Count > b.children.Count ? b : a; }

	public static RNode MinimizeArea(RNode a, RNode b)
	{
		float a_area = a.mbr.Area();
		float b_area = b.mbr.Area();

		if(a_area == b_area)
		{ return MinimizeFullness(a, b); }
		return a_area > b_area ? b : a;
	}

	public static RNode MinimizeGrowth(RNode a, RNode b, IBoundable incl)
	{
		Bounds a_incl = a.mbr;
		a_incl.Encapsulate(incl.mbr);
		Bounds b_incl = b.mbr;
		b_incl.Encapsulate(incl.mbr);

		float a_growth = a_incl.Area() - a.mbr.Area();
		float b_growth = b_incl.Area() - b.mbr.Area();

		if(a_growth == b_growth)
		{ return MinimizeArea(a, b); }
		return a_growth > b_growth ? b : a;
	}

	public void RecalculateMBR()
	{
		if(fill == 0){ _mbr = new Bounds(Vector3.zero, Vector3.zero); return; }

		Vector3 min = _children[0].mbr.min;
		Vector3 max = _children[0].mbr.max;

		for(int i = 1; i < _children.Count; i++)
		{
			Bounds bound = _children[i].mbr;
			min.x = Mathf.Min(min.x, bound.min.x);
			min.z = Mathf.Min(min.z, bound.min.z);
			max.x = Mathf.Max(max.x, bound.max.x);
			max.z = Mathf.Max(max.z, bound.max.z);
		}

		_mbr = new Bounds((min+max)*0.5f, max-min);
	}
	
	public void Add(IBoundable entry)
	{
		_children.Add(entry);
		RecalculateMBR();
	}
	
	public void Remove(IBoundable entry)
	{
		_children.Remove(entry);
		RecalculateMBR();
	}
	
	public void SplitChild(RNode child)
	{		
		IBoundable e1 = child.children[0];
		IBoundable e2 = child.children[1];
		float max_dist = (e1.mbr.center - e2.mbr.center).magnitude;

		for(int i = 0; i < child.children.Count; i++)
		{
			IBoundable e1_cand = child.children[i];
			for(int j = 0; j < child.children.Count; j++)
			{
				if(i == j){ continue; }

				IBoundable e2_cand = child.children[j];
				float dist = (e1_cand.mbr.center - e2_cand.mbr.center).magnitude;

				if(dist > max_dist)
				{
					e1 = e1_cand;
					e2 = e2_cand;
					max_dist = dist;
				}
			}
		}
				
		RNode a = new RNode(new List<IBoundable>{e1}, child.level);
		RNode b = new RNode(new List<IBoundable>{e2}, child.level);
		child.children.Remove(e1);
		child.children.Remove(e2);

		for(int i = 0; i < child.children.Count; i++)
		{
			IBoundable e = child.children[i];
			if(e != e1 && e != e2)
			{
				RNode destination = RNode.MinimizeGrowth(a, b, e);
				destination.children.Add(e);
				child.children.Remove(e);
				i--;
			}
		}
		
		a.RecalculateMBR();
		b.RecalculateMBR();
		
		_children.Remove(child);
		_children.Add(a);
		_children.Add(b);
	}

	public RNode(List<IBoundable> children, int level)
	{
		_children = children == null ? new List<IBoundable>() : children;
		_level = level;
		RecalculateMBR();
	}
}
