using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	int damage;
	[SerializeField]
	bool lethal;
	
	Combatant wielder;
	
	void Awake()
	{
		wielder = transform.root.GetComponentInChildren<Combatant>();
	}
	
    void OnTriggerEnter(Collider collider)
	{
		if(wielder != null)
		{
			Combatant target = collider.GetComponent<Combatant>();
			if(target != null){ target.EnqueueAttack(new Attack(wielder, Vector3.zero, damage, lethal)); }
		}
	}
}