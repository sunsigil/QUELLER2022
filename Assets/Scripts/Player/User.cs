using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    Controller controller;
	Animator animator;
	Spellcaster spellcaster;
	Looker looker;
	Runner runner;
	Shooter shooter;

    void Awake()
    {
        controller = GetComponent<Controller>();
		animator = GetComponentInChildren<Animator>();
		spellcaster = GetComponent<Spellcaster>();
		looker = GetComponent<Looker>();
		runner = GetComponent<Runner>();
		shooter = GetComponent<Shooter>();
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
