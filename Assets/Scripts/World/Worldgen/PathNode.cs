using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour, IBoundable
{
	MeshRenderer cell_mesh;

	PathNode[] _neighbourhood;
	public PathNode[] neighbourhood => _neighbourhood;
	
	public Bounds mbr => cell_mesh.bounds;
	
	void Awake()
	{ cell_mesh = GetComponent<MeshRenderer>(); }
}