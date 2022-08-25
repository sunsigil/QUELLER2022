using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Matrix3x3
{
    float[][] n;

	public Matrix3x3(
		float n11, float n12, float n13,
		float n21, float n22, float n23,
		float n31, float n32, float n33)
	{
		n = new float[][]
		{
			new float[]{n11, n12, n13},
			new float[]{n21, n22, n23},
			new float[]{n31, n32, n33}
		};
	}
	
	public Matrix3x3(Vector3 r1, Vector3 r2, Vector3 r3)
	{
		n = new float[][]
		{
			new float[]{r1.x, r1.y, r1.z},
			new float[]{r2.x, r2.y, r2.z},
			new float[]{r3.x, r3.y, r3.z}
		};
	}
	
	public Matrix3x3 zero => new Matrix3x3(0,0,0,0,0,0,0,0,0);
	public Matrix3x3 I => new Matrix3x3(1,0,0,0,1,0,0,0,1);
	public Matrix3x3 one => new Matrix3x3(1,1,1,1,1,1,1,1,1);
	
	public float Get(int i, int j)
	{ return n[i-1][j-1]; }
	public Vector3 GetRow(int i)
	{ return new Vector3(n[i-1][0], n[i-1][1], n[i-1][2]); }
	public Vector3 GetCol(int j)
	{ return new Vector3(n[0][j-1], n[1][j-1], n[2][j-1]); }
	
	public void Set(int i, int j, float val)
	{ n[i-1][j-1] = val; }
	public void SetRow(int i, Vector3 val)
	{ n[i-1][0] = val.x; n[i-1][1] = val.y; n[i-1][2] = val.z; }
	public void SetCol(int j, Vector3 val)
	{ n[0][j-1] = val.x; n[1][j-1] = val.y; n[2][j-1] = val.z; }
	
	public Matrix3x3 MulBy(float l)
	{
		return new Matrix3x3
		(
			l * GetRow(1),
			l * GetRow(2),
			l * GetRow(3)
		);
	}
	
	public Vector3 Mul(Vector3 v)
	{
		return new Vector3
		(
			Vector3.Dot(GetRow(1), v),
			Vector3.Dot(GetRow(2), v),
			Vector3.Dot(GetRow(2), v)
		);
	}
	
	public Matrix3x3 Add(Matrix3x3 B)
	{
		return new Matrix3x3
		(
			GetRow(1) + B.GetRow(1),
			GetRow(2) + B.GetRow(2),
			GetRow(3) + B.GetRow(3)
		);
	}
	
	public Matrix3x3 Sub(Matrix3x3 B)
	{ return Add(B.MulBy(-1)); }
	
	public Matrix3x3 Mul(Matrix3x3 B)
	{
		return new Matrix3x3
		(
			Vector3.Dot(GetRow(1), B.GetCol(1)), Vector3.Dot(GetRow(1), B.GetCol(2)), Vector3.Dot(GetRow(1), B.GetCol(3)),
			Vector3.Dot(GetRow(2), B.GetCol(1)), Vector3.Dot(GetRow(2), B.GetCol(2)), Vector3.Dot(GetRow(2), B.GetCol(3)),
			Vector3.Dot(GetRow(3), B.GetCol(1)), Vector3.Dot(GetRow(3), B.GetCol(2)), Vector3.Dot(GetRow(3), B.GetCol(3))
		);
	}
	
	float Det2x2(float a, float b, float c, float d)
	{ return a*d-b*c; }
	
	public float Determinant()
	{
		return
		n[0][0] * Det2x2(n[1][1], n[1][2], n[2][1], n[2][2]) -
		n[0][1] * Det2x2(n[1][0], n[1][2], n[2][0], n[2][2]) +
		n[0][2] * Det2x2(n[1][0], n[1][1], n[2][0], n[2][1]);
	}
}
