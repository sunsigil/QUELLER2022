using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
	GameObject prefab;
	[SerializeField]
	Color colour = Color.green;
	
	GameObject instance;

	public void Reorient()
	{
		instance.transform.position = transform.position;
		instance.transform.rotation = transform.rotation;
	}

	public void Respawn()
    {
		Destroy(instance);
		instance = Instantiate(prefab);
		Reorient();
	}

    void OnDrawGizmos()
	{
		Gizmos.color = colour;
		Gizmos.DrawCube(transform.position, new Vector3(1, 2, 1));
		Gizmos.DrawLine(transform.position, transform.position + transform.forward);
	}
}
