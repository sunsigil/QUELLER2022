using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline
{
	float duration;
	float timer;
	public float progress => (duration != 0) ? (timer / duration) : 1;

	public float Tick(float dt)
	{
		timer = Mathf.Clamp(timer + dt, 0, duration);
		return progress;
	}

	public Timeline(float duration)
	{
		this.duration = duration;
		timer = 0;
	}
}
