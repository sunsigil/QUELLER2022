using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
	AudioWizard audio_wizard;

    Controller controller;
	Animator animator;
	Looker looker;
	Runner runner;
	Shooter shooter;

	ISpellcaster[] casters;
	int caster_index;

	void Awake()
    {
		audio_wizard = FindObjectOfType<AudioWizard>();

        controller = GetComponent<Controller>();
		animator = GetComponentInChildren<Animator>();
		looker = GetComponent<Looker>();
		runner = GetComponent<Runner>();
		shooter = GetComponent<Shooter>();

		casters = GetComponents<ISpellcaster>();
		caster_index = 1;
	}

    void Start()
    {
		runner.on_grounded.AddListener(delegate { print("ya"); animator.SetTrigger("Landed"); });
		audio_wizard.PlayEffect("bb_spawn");
    }

    // Update is called once per frame
    void Update()
    {
		if(controller.Pressed(InputCode.SWITCH_LEFT))
		{ 
			caster_index = (caster_index - 1) % casters.Length;
			animator.SetTrigger("Switched");
		}
		if (controller.Pressed(InputCode.SWITCH_RIGHT))
		{
			caster_index = (caster_index + 1) % casters.Length;
			animator.SetTrigger("Switched");
		}

		if (controller.Pressed(InputCode.RHAND))
		{ shooter.Fire(); }

		if (controller.Held(InputCode.LHAND))
		{
			casters[caster_index].Charge(Time.deltaTime * 2);
			animator.SetBool("Firing", true);
		}
		else if (controller.Released(InputCode.LHAND))
		{
			casters[caster_index].Cast();
			animator.SetBool("Firing", false);
		}

		if(controller.Pressed(InputCode.CONFIRM))
        {
			FindObjectOfType<HUD>().PushNotification("AIEEEEEEE!!!!");
			GetComponent<Combatant>().EnqueueAttack(new Attack(null, Vector3.zero, 10, true));
        }

		animator.SetBool("Running", runner.running);
    }
}
