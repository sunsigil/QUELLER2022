using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Halfedge
{
	float[] n;
	
	Vector2 _start;
	public Vector2 start => _start;
	Vector2 _reach;
	public Vector2 reach => _reach;
	Vector2 _end;
	public Vector2 end => _end;
	
	Vector2 _left;
	public Vector2 left => _left;
	Vector2 _right;
	public Vector2 right => _right;
	Vector2 _bottom;
	public Vector2 bottom => _bottom;
	Vector2 _top;
	public Vector2 top => _top;
	Vector2 _min;
	public Vector2 min => _min;
	Vector2 _max;
	public Vector2 max => _max;
	
	public Halfedge(float sx, float sy, float rx, float ry)
	{
		n = new float[4]{ sx, sy, rx, ry };
		
		_start = new Vector2(sx, sy);
		_reach = new Vector2(rx, ry);
		_end = _start + _reach;
		
		_left = _end.x < _start.x ? _end : _start;
		_right = _left == _end ? _start : _end;
		_bottom = _end.y < _start.y ? _end : _start;
		_top = _bottom == _end ? _start : _end;
		_min = new Vector2(_left.x, _bottom.y);
		_max = new Vector2(_right.x, _top.y);
	}
	
	public Halfedge(Vector2 start, Vector2 reach) : this(start.x, start.y, reach.x, reach.y){}
	public Halfedge(Vector4 v) : this(v.x, v.y, v.z, v.w){}
	
	public static bool operator ==(Halfedge l, Halfedge r)
	{
		return
		(l.start == r.start) &&
		((l.reach == r.reach) ||
		(l.end == r.end));
	}
	public static bool operator !=(Halfedge l, Halfedge r)
	{ return !(l == r); }
	
	public Halfedge ScaleBy(float l)
	{ return new Halfedge(l*n[0], l*n[1], l*n[2], l*n[3]); }
	
	public Halfedge ScaleBy(Vector2 l)
	{ return new Halfedge(l.x*n[0], l.y*n[1], l.x*n[2], l.y*n[3]); }
	
	public Halfedge ScaleBy(Vector4 l)
	{ return new Halfedge(l.x*n[0], l.y*n[1], l.z*n[2], l.w*n[3]); }
	
	public Halfedge ShiftStart(Vector2 v)
	{ return new Halfedge(n[0]+v.x, n[1]+v.y, n[2], n[3]); }
	
	public Halfedge ShiftEnd(Vector2 v)
	{ return new Halfedge(n[0], n[1], n[2]+v.x, n[3]+v.y); }
	
	public Halfedge ScaleReach(float l)
	{ return new Halfedge(n[0], n[1], l*n[2], l*n[3]); }
	
	public Halfedge ScaleReach(Vector2 l)
	{ return new Halfedge(n[0], n[1], l.x*n[2], l.y*n[3]); }
}
