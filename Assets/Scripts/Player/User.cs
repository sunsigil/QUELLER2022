using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
	AudioWizard audio_wizard;

    Controller controller;
	Animator animator;
	Spellcaster spellcaster;
	Looker looker;
	Runner runner;
	Shooter shooter;

    void Awake()
    {
		audio_wizard = FindObjectOfType<AudioWizard>();

        controller = GetComponent<Controller>();
		animator = GetComponentInChildren<Animator>();
		spellcaster = GetComponent<Spellcaster>();
		looker = GetComponent<Looker>();
		runner = GetComponent<Runner>();
		shooter = GetComponent<Shooter>();
    }

    void Start()
    {
		runner.on_grounded.AddListener(delegate { print("ya"); animator.SetTrigger("Landed"); });
		audio_wizard.PlayEffect("bb_spawn");
    }

    // Update is called once per frame
    void Update()
    {
		if (controller.Pressed(InputCode.RHAND))
		{ shooter.Fire(); }

		if (controller.Held(InputCode.LHAND))
		{
			spellcaster.Charge(Time.deltaTime * 2);
			animator.SetBool("Firing", true);
		}
		else if (controller.Released(InputCode.LHAND))
		{
			spellcaster.Cast();
			animator.SetBool("Firing", false);
		}

		animator.SetBool("Running", runner.running);
    }
}
