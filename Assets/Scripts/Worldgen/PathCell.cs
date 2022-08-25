using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCell : MonoBehaviour, IBoundable
{
	MeshRenderer cell_mesh;
	
	public Bounds mbr => cell_mesh.bounds;
	
	void Awake()
	{ cell_mesh = GetComponent<MeshRenderer>(); }
}