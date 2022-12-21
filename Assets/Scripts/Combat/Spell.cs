using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spell : MonoBehaviour
{
	[Header("Damage")]

	[SerializeField]
	int damage;
	[SerializeField]
	bool lethal;

	ParticleSystem particle;
	Collider collider;

	Combatant sender;
	Combatant target;
	Vector3 direction;
	float speed;

	UnityEvent on_hit;

	public void Sleep()
    {
		collider.enabled = false;

		if(particle != null)
		{ particle.Pause(); }
	}

	public void Wake()
    {
		collider.enabled = true;

		if (particle != null && !particle.isPlaying)
		{ particle.Play(); }
	}

	public void Target(Combatant target, float speed)
	{
		transform.parent = null;
		this.target = target;
		this.speed = speed;
	}

	public void Launch(Vector3 velocity)
	{
		transform.parent = null;
		speed = velocity.magnitude;
		direction = velocity.normalized;
	}

	void Awake()
	{
		particle = GetComponent<ParticleSystem>();
		collider = GetComponent<Collider>();

		sender = transform.root.GetComponentInChildren<Combatant>();
		target = null;
		direction = Vector3.zero;
		speed = 0;

		on_hit = new UnityEvent();
		on_hit.AddListener(delegate { Destroy(gameObject); });
	}

	void FixedUpdate()
    {
		if(target != null)
        {
			Vector3 line = target.transform.position - transform.position;
			direction = line.normalized;
			transform.forward = direction;
        }

		transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
	}
	
    void OnTriggerEnter(Collider collider)
	{
		if(sender != null)
		{
			Combatant target = collider.GetComponent<Combatant>();
			if(target != null)
			{
				bool hit = target.EnqueueAttack(new Attack(sender, direction * speed, damage, lethal));
				if(hit)
				{ on_hit.Invoke(); }
			}
		}
	}
}