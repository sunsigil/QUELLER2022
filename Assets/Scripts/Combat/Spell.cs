using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spell : MonoBehaviour
{
	[Header("Aesthetics")]

	[SerializeField]
	string cast_effect;

	[Header("Motion")]

	[SerializeField]
	float smoothing;

	[Header("Damage")]

	[SerializeField]
	int damage;
	[SerializeField]
	bool lethal;

	AudioWizard audio_wizard;

	ParticleSystem particle;
	Collider collider;

	Combatant sender;
	Transform target;
	Vector3 velocity;

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

	public void Target(Transform target)
	{
		transform.parent = null;
		this.target = target;
		audio_wizard.PlayEffect(cast_effect);
	}

	public void Launch(Vector3 velocity)
	{
		transform.parent = null;
		this.velocity = velocity;
		audio_wizard.PlayEffect(cast_effect);
	}

	void Awake()
	{
		audio_wizard = FindObjectOfType<AudioWizard>();

		particle = GetComponent<ParticleSystem>();
		collider = GetComponent<Collider>();

		sender = transform.root.GetComponentInChildren<Combatant>();
		target = null;
		velocity = Vector3.zero;

		on_hit = new UnityEvent();
		on_hit.AddListener(delegate { Destroy(gameObject); });
	}

	void FixedUpdate()
    {
		if(target != null)
        {
			Vector3 line = target.position - transform.position;
			velocity = Vector3.Lerp(velocity, line, 1 - smoothing);
        }

		transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
	}
	
    void OnTriggerEnter(Collider collider)
	{
		if(sender != null)
		{
			Combatant target = collider.GetComponent<Combatant>();
			if(target != null)
			{
				bool hit = target.EnqueueAttack(new Attack(sender, velocity, damage, lethal));
				if(hit)
				{ on_hit.Invoke(); }
			}
		}
	}
}