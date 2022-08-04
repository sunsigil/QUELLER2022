using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
	[SerializeField]
	GameObject deathblow_dot;
	
	AudioWizard audio_wizard;
	
	Combatant combatant;
	Machine machine;
	
	Timeline timeline;
	
	void Hurt()
	{ print("Ouch!"); }
	
	void Die()
	{ 
		audio_wizard.PlayEffect("death");
		
		Destroy(gameObject);
	}
	
	void Active(StateSignal signal)
	{
		switch(signal)
		{
			case StateSignal.FIXED_TICK:
				transform.Rotate(Vector3.up * Time.fixedDeltaTime);
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
	}
	
	void Start()
	{
		combatant.on_hurt.AddListener(Hurt);
		combatant.on_deplete.AddListener(delegate{ machine.Transition(Depleted); });
		combatant.on_die.AddListener(Die);
	}
}
