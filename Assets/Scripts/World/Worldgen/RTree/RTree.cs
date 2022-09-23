using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTree<T> where T : class, IBoundable
{
	const int t = 3;
	int M => t*2-1;

    RNode head;
	
	public int height => head.level;
	public int overload => head.fill - M;
	
	void SplitHead()
	{
		RNode new_head = new RNode(new List<IBoundable>{head}, head.level+1);
		new_head.SplitChild(head);
		head = new_head;
	}

	void Insert_RE(T entry, RNode node)
	{
		if(node == head && node.is_leaf)
		{
			node.Add(entry);
			if(node.fill > M)
			{ SplitHead(); }
			return;
		}
		
		RNode best = node.children[0] as RNode;
		for(int i = 0; i < node.fill; i++)
		{
			RNode candidate = node.children[i] as RNode;
			best = RNode.MinimizeGrowth(best, candidate, entry);
		}
		
		if(best.is_leaf)
		{ best.Add(entry); }
		else
		{ Insert_RE(entry, best); }
	
		for(int i = 0; i < node.fill; i++)
		{
			RNode child = node.children[i] as RNode;
			if(child.fill > M)
			{ node.SplitChild(child); }
		}
	}
	public void Insert(T entry)
	{
		Insert_RE(entry, head);
		if(head.fill > M)
		{ SplitHead(); }
	}

	void AreaSearch_RE(Bounds area, RNode node, List<T> accumulator)
	{
		if(node.is_leaf) // LEAF
		{
			foreach(IBoundable data in node.children)
			{
				if(data.mbr.Intersects(area))
				{ accumulator.Add(data as T); }
			}
		}
		else // BRANCH
		{
			foreach(RNode child in node.children)
			{
				if(child.mbr.Intersects(area))
				{
					AreaSearch_RE(area, child, accumulator);
				}
			}
		}
	}
	public List<T> AreaSearch(Bounds area)
	{
		List<T> accumulator = new List<T>();
		AreaSearch_RE(area, head, accumulator);
		return accumulator;
	}

	void Elements_RE(RNode node, List<RNode> accumulator)
	{
		if(node.is_leaf)
		{ accumulator.Add(node); }
		else
		{
			foreach(RNode child in node.children)
			{
				Elements_RE(child, accumulator);
			}
		}
	}
	public List<RNode> Elements()
	{
		List<RNode> accumulator = new List<RNode>(){head};
		Elements_RE(head, accumulator);
		return accumulator;
	}

	public List<T> Entries()
	{ return AreaSearch(head.mbr); }

	public RTree()
	{
		head = new RNode(null, 0);
	}
}
