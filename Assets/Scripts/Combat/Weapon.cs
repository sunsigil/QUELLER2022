using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	protected int damage;
	[SerializeField]
	protected bool lethal;

	protected Combatant wielder;

	protected Vector3 _velocity;
	public Vector3 velocity
	{
		get => _velocity;
		set => _velocity = value;
	}

	protected UnityEvent on_hit;

	protected virtual void Awake()
	{
		wielder = transform.root.GetComponentInChildren<Combatant>();

		on_hit = new UnityEvent();
	}
	
    protected void OnTriggerEnter(Collider collider)
	{
		if(wielder != null)
		{
			Combatant target = collider.GetComponent<Combatant>();
			if(target != null)
			{
				bool hit = target.EnqueueAttack(new Attack(wielder, _velocity, damage, lethal));
				if(hit)
				{ on_hit.Invoke(); }
			}
		}
	}
}