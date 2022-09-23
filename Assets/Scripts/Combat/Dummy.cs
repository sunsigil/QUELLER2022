using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combatant))]
[RequireComponent(typeof(Machine))]
[RequireComponent(typeof(Rigidbody))]
public class Dummy : MonoBehaviour
{
	[SerializeField]
	GameObject deathblow_dot;
	[SerializeField]
	GameObject target_ring;
	
	AudioWizard audio_wizard;
	
	Combatant combatant;
	Machine machine;
	Rigidbody rigidbody;

	Transform player;
	bool tracking;
	Vector3 track_ray;

	Timeline timeline;
	
	void Hurt()
	{ print("Ouch!"); }
	
	void Die()
	{ 
		audio_wizard.PlayEffect("bb_gore");
		Destroy(gameObject);
	}
	
	void Active(StateSignal signal)
	{
		switch(signal)
		{
			case StateSignal.TICK:
				tracking = false;

				if(player == null)
				{ return; }

				track_ray = player.position - transform.position;
				transform.forward = Vector3.Scale(track_ray, new Vector3(1, 0, 1)).normalized;
				
				RaycastHit hit;
				if(Physics.Raycast(transform.position, track_ray, out hit))
                {
					if(hit.transform == player)
                    { tracking = true; }
                }
			break;

			case StateSignal.FIXED_TICK:
				if(tracking)
                {
					rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime);
                }
			break;
		}
	}
	
	void Depleted(StateSignal signal)
	{		
		switch(signal)
		{
			case StateSignal.ENTER:
				timeline = new Timeline(2);
				deathblow_dot.SetActive(true);
			break;
			
			case StateSignal.TICK:
				if(timeline.Tick(Time.deltaTime) >= 1)
				{ 
					combatant.Heal(1);
					machine.Transition(Active);
				}
				
			break;
			
			case StateSignal.EXIT:
				deathblow_dot.SetActive(false);
			break;
		}
	}
	
    void Awake()
	{
		audio_wizard = FindObjectOfType<AudioWizard>();
		
		combatant = GetComponent<Combatant>();
		machine = GetComponent<Machine>();
		rigidbody = GetComponent<Rigidbody>();
	}
	
	void Start()
	{
		combatant.on_hurt.AddListener(Hurt);
		combatant.on_deplete.AddListener(delegate{ machine.Transition(Depleted); });
		combatant.on_die.AddListener(Die);
		
		combatant.on_targeted.AddListener(delegate{ target_ring.SetActive(true); });
		combatant.on_untargeted.AddListener(delegate{ target_ring.SetActive(false); });

		player = FindObjectOfType<User>().transform;

		machine.Transition(Active);
	}
}
