using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTree<T> where T : class, IBoundable
{
	const int M = 5;

    RNode head;

	RNode[] Split(RNode node)
	{
		IBoundable e1 = node.children[0];
		IBoundable e2 = node.children[1];
		float max_dist = (e1.MBR().center - e2.MBR().center).magnitude;

		for(int i = 0; i < node.children.Count; i++)
		{
			IBoundable e1_cand = node.children[i];
			for(int j = 0; j < node.children.Count; j++)
			{
				if(i == j){ continue; }

				IBoundable e2_cand = node.children[j];
				float dist = (e1_cand.MBR().center - e2_cand.MBR().center).magnitude;

				if(dist > max_dist)
				{
					e1 = e1_cand;
					e2 = e2_cand;
					max_dist = dist;
				}
			}
		}

		RNode a = new RNode(new List<IBoundable>{e1}, true);
		RNode b = new RNode(new List<IBoundable>{e2}, true);
		node.children.Remove(e1); node.children.Remove(e2);

		for(int i = 0; i < node.children.Count; i++)
		{
			IBoundable e = node.children[i];

			if(e != e1 && e != e2)
			{
				RNode destination = RNode.MinimizeGrowth(a, b, e);
				destination.children.Add(e);
				destination.RecalculateMBR();

				node.children.Remove(e);
				i--;
			}
		}

		node.RecalculateMBR();

		return new RNode[]{a, b};
	}

	void Insert_RE(T entry, RNode node)
	{
		if(node.is_leaf)
		{
			node.children.Add(entry);
			node.RecalculateMBR();
		}
		else
		{
			RNode best = node.children[0] as RNode;
			for(int i = 0; i < node.children.Count; i++)
			{
				RNode candidate = node.children[i] as RNode;
				best = RNode.MinimizeGrowth(best, candidate, entry);
			}

			Insert_RE(entry, best);

			for(int i = 0; i < node.children.Count; i++)
			{
				RNode child = node.children[i] as RNode;
				if(child.children.Count > M)
				{
					RNode[] siblings = Split(child);
					node.children.Add(siblings[0]);
					node.children.Add(siblings[1]);

					node.children.Remove(child);
				}
			}
		}
	}
	public void Insert(T entry)
	{ Insert_RE(entry, head); }

	void AreaSearch_RE(Bounds area, RNode node, List<T> accumulator)
	{
		if(node.is_leaf) // LEAF
		{
			foreach(IBoundable data in node.children)
			{
				if(data.MBR().Intersects(area))
				{ accumulator.Add(data as T); }
			}
		}
		else // BRANCH
		{
			foreach(RNode child in node.children)
			{
				if(child.MBR().Intersects(area))
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

	void Entries_RE(RNode node, List<T> accumulator)
	{
		if(node.is_leaf)
		{
			foreach(IBoundable entry in node.children)
			{ accumulator.Add(entry as T); }
		}
		else
		{
			foreach(RNode child in node.children)
			{
				Entries_RE(child, accumulator);
			}
		}
	}
	public List<T> Entries()
	{
		List<T> accumulator = new List<T>();
		Entries_RE(head, accumulator);
		return accumulator;
	}

	public RTree()
	{
		RNode subhead = new RNode(null, true);
		head = new RNode(new List<IBoundable>(){subhead}, false);
	}
}
