using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Runner : MonoBehaviour
{
	[SerializeField]
	float grounding_tolerance;
	[SerializeField]
	float bumping_tolerance;

	[SerializeField]
	float run_speed;
	[SerializeField]
	Vector2 jump_thrust;
	[SerializeField]
	float air_control;
	[SerializeField]
	float gravity;

	Controller controller;
	Rigidbody rigidbody;
	CapsuleCollider capsule_collider;

	Vector3 run_vel;
	Vector3 jump_vel;
	Vector3 vel => run_vel + jump_vel;
	public bool running => run_vel.magnitude >= 0.1f;

	float ground_dist;
	bool grounded;
	bool last_grounded;
	UnityEvent _on_grounded;
	public UnityEvent on_grounded => _on_grounded;

	float bump_dist;
	bool last_bumping;
	bool bumping;

	void CheckGrounding()
	{
		last_grounded = grounded;

		RaycastHit hit;
		if (Physics.Raycast(
			transform.TransformPoint(capsule_collider.center),
			-transform.up,
			out hit,
			Mathf.Infinity,
			~LayerMask.NameToLayer("Floor")
		))
		{
			ground_dist = hit.distance - (capsule_collider.height * 0.5f);
			grounded = ground_dist < grounding_tolerance;
		}
		else { grounded = false; }

		if(!last_grounded && grounded)
		{ _on_grounded.Invoke(); }
	}

	void CheckBumping()
	{
		last_bumping = bumping;

		RaycastHit hit;
		if (Physics.Raycast(
			transform.TransformPoint(capsule_collider.center),
			Vector3.Scale(vel, new Vector3(1, 0, 1)),
			out hit,
			Mathf.Infinity,
			~LayerMask.NameToLayer("Wall")
		))
		{
			bump_dist = hit.distance - capsule_collider.radius;
			bumping = bump_dist < bumping_tolerance;
		}
		else { bumping = false; }
	}

	void Awake()
	{
		controller = GetComponent<Controller>();
		rigidbody = GetComponent<Rigidbody>();
		capsule_collider = GetComponent<CapsuleCollider>();

		_on_grounded = new UnityEvent();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 run_input = transform.forward * controller.InputValue("Vertical", true);
		run_input += transform.right * controller.InputValue("Horizontal", true);
		run_input.Normalize();

		if (grounded)
		{
			if (!Mathf.Approximately(run_input.magnitude, 0))
			{ run_vel = run_input * run_speed; }
			else { run_vel = Vector3.zero; }
		}
		else
		{ run_vel = run_input * run_speed * air_control; }

		if (controller.Pressed(InputCode.JUMP) && grounded)
		{ jump_vel = run_input * jump_thrust.x + Vector3.up * jump_thrust.y; }
	}

	void FixedUpdate()
	{
		CheckGrounding();
		CheckBumping();

		if (!grounded)
		{ jump_vel -= Vector3.up * gravity * Time.fixedDeltaTime; }
		else if (!last_grounded)
		{ jump_vel = Vector3.zero; }

		if (bumping)
		{
			run_vel = Vector3.zero;

			if (!last_bumping)
			{ jump_vel = Vector3.zero; }
		}

		rigidbody.MovePosition(transform.position + vel * Time.fixedDeltaTime);
	}
}
