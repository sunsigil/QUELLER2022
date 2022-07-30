using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
	GameObject prefab;
	
	GameObject instance;
	
	public void Orient()
	{
		instance.transform.position = transform.position;
		instance.transform.rotation = transform.rotation;
	}
	
	void Awake()
	{
		instance = Instantiate(prefab);
		Orient();
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(transform.position, new Vector3(1, 2, 1));
		Gizmos.DrawLine(transform.position, transform.position + transform.forward);
	}
}
