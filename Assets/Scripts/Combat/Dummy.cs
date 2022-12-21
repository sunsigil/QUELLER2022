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
	
	AudioWizard audio_wizard;
	
	Combatant combatant;
	Machine machine;
	Rigidbody rigidbody;
	Targeter targeter;

	Timeline timeline;
	
	void Die()
	{ 
		audio_wizard.PlayEffect("bb_gore");
		Destroy(gameObject);
	}
	
	void Active(StateSignal signal)
	{
		switch(signal)
		{
			case StateSignal.FIXED_TICK:
				if(targeter.target != null)
                {
					Vector3 track_ray = targeter.target.transform.position - transform.position;
					transform.forward = Vector3.Scale(track_ray, new Vector3(1, 0, 1)).normalized;
					rigidbody.MovePosition(transform.position + transform.forward * 5 * Time.fixedDeltaTime);
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
		targeter = GetComponent<Targeter>();
	}
	
	void Start()
	{
		combatant.on_deplete.AddListener(delegate{ machine.Transition(Depleted); });
		combatant.on_die.AddListener(Die);

		machine.Transition(Active);
	}
}
