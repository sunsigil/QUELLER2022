/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLeaf<T> : RNode<T>
{	
	RNode<T> _parent;
	public RNode<T> parent
	{
		get => _parent;
		set => _parent = value;
	}	
	
	List<T> _contents;
	public List<T> contents
	{
		get => _contents;
		set => _contents = value;
	}
	
	public bool is_leaf => T is RNode;
	
	public static Bounds CombinedMBR<B>(List<B> items) where B : IBoundable
	{	
		if(items.Count == 0){ return new Bounds(Vector3.zero, Vector3.zero); }
		
		Vector3 min = items[0].MBR().min;
		Vector3 max = items[0].MBR().max;
			
		for(int i = 1; i < items.Count; i++)
		{
			Bounds bound = items[i].MBR();
			min.x = Mathf.Min(min.x, bound.min.x);
			min.z = Mathf.Min(min.z, bound.min.z);
			max.x = Mathf.Max(max.x, bound.max.x);
			max.z = Mathf.Max(max.z, bound.max.z);
		}
		
		return new Bounds((min+max)*0.5f, max-min);
	}
	
	public static RNode<T> MinimizeFullness(RNode<T> a, RNode<T> b)
	{ return a.contents.Count > b.contents.Count ? b : a; }
	
	public static RNode<T> MinimizeArea(RNode<T> a, RNode<T> b)
	{
		float a_area = a.MBR().Area();
		float b_area = b.MBR().Area();
		
		if(a_area == b_area)
		{ return MinimizeFullness(a, b); }
		return a_area > b_area ? b : a;
	}
	
	public static RNode<T> MinimizeGrowth(RNode<T> a, RNode<T> b, T incl)
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
	{ _mbr = is_leaf ? CombinedMBR(_chi) : CombinedMBR(_children); }
	
	public RNode(RNode<T> parent, List<RNode<T>> children)
	{
		_parent = parent;
		_children = children;
		_contents = null;
	}
	
	public RNode(RNode<T> parent, List<T> contents)
	{
		_parent = parent;
		_children = null;
		_contents = contents;
	}
}*/
