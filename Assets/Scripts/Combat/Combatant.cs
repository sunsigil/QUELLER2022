using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Combatant : MonoBehaviour
{
	[SerializeField]
	Faction _faction;
	public Faction faction => _faction;

	[SerializeField]
	int _max_lives;
	public int max_lives => _max_lives;
	int _lives;
	public int lives => _lives;
	public float life => (float)_lives / (float)_max_lives;

	[SerializeField]
	float hurt_cooldown;
	Timeline nohurt_timeline;
	bool invincible;

	Queue<Attack> incoming;
	UnityEvent _on_hurt;
	public UnityEvent on_hurt => _on_hurt;
	UnityEvent _on_deplete;
	public UnityEvent on_deplete => _on_deplete;
	UnityEvent _on_die;
	public UnityEvent on_die => _on_die;
	
	Combatant _adversary;
	public Combatant adversary => _adversary;
	UnityEvent _on_targeted;
	public UnityEvent on_targeted => _on_targeted;
	UnityEvent _on_untargeted;
	public UnityEvent on_untargeted => _on_untargeted;

	void ProcessAttack(Attack attack)
	{
		int old_lives = _lives;
		_lives = Mathf.Clamp(_lives - attack.damage, 0, _max_lives);

		_on_hurt.Invoke();

		if(_lives == 0)
		{
			if(old_lives != 0)
			{
				_on_deplete.Invoke();
			}
			else if(attack.lethal)
			{
				_on_die.Invoke();
			}
		}
	}

	public void ToggleInvincible(bool toggle)
	{ invincible = toggle; }

	public void Heal(int quant)
	{ _lives = Mathf.Clamp(_lives + quant, 0, _max_lives); }

	public bool EnqueueAttack(Attack attack)
	{
		if(invincible){ return false; }
		if(nohurt_timeline.progress < 1){ return false; }

		if(attack.sender == this){ return false; }
		if(attack.sender.faction == _faction){ return false; }

		incoming.Enqueue(attack);
		nohurt_timeline = new Timeline(hurt_cooldown);

		return true;
	}
	
	public void Target(Combatant other)
	{
		if(other == this){ return; }
		if(other == _adversary){ return; }
		
		if(_adversary != null)
		{ _adversary.on_untargeted.Invoke(); }

		if((_adversary = other) != null)
		{ _adversary.on_targeted.Invoke(); }
	}

	void Awake()
	{
		_lives = _max_lives;

		nohurt_timeline = new Timeline(hurt_cooldown);
		incoming = new Queue<Attack>();

		_on_hurt = new UnityEvent();
		_on_deplete = new UnityEvent();
		_on_die = new UnityEvent();
		
		_on_targeted = new UnityEvent();
		_on_untargeted = new UnityEvent();
	}

	void Update()
	{
		nohurt_timeline.Tick(Time.deltaTime);

		if(incoming.Count > 0)
		{ ProcessAttack(incoming.Dequeue()); }
	}
}
