using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour, ISavable
{
	[Header("Smoothing")]
	
	[SerializeField]
	float grounding_tolerance;
	[SerializeField]
	float bumping_tolerance;
	
    [Header("Running")]

    [SerializeField]
    float run_speed;
	[SerializeField]
	float halting_power;
	
	[SerializeField]
	Vector2 jump_thrust;
	[SerializeField]
	float air_control;
	[SerializeField]
	float gravity;

    [Header("Camera")]

    [SerializeField]
    float pitch_range;
    [SerializeField]
    float pitch_sensitivity;
    [SerializeField]
    float yaw_sensitivity;

    [SerializeField]
    Camera camera;

    Controller controller;
    Rigidbody rigidbody;
	CapsuleCollider capsule_collider;
	
	float frame_yaw;
	float frame_pitch;
	
    Vector3 run_vel;
	Vector3 jump_vel;
	Vector3 vel => run_vel + jump_vel;
	
	float ground_dist;
	bool grounded;
	bool last_grounded;
	
	float bump_dist;
	bool last_bumping;
	bool bumping;
	
	void CheckGrounding()
	{
		last_grounded = grounded;
				
		RaycastHit hit;
		if(Physics.Raycast(
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
		else{ grounded = false; }
	}
	
	void CheckBumping()
	{			
		last_bumping = bumping;
		
		RaycastHit hit;
		if(Physics.Raycast(
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
		else{ bumping = false; }
	}

    public void LoadDefaults()
    {
        rigidbody.MovePosition(Vector3.zero);
    }

    public string WriteBlock()
    {
        return $"{transform.position.x} {transform.position.y} {transform.position.z}";
    }

    public bool ReadBlock(string block)
    {
        try
        {
            string[] pos_strs = block.Split(' ');
            float x = float.Parse(pos_strs[0]);
            float y = float.Parse(pos_strs[1]);
            float z = float.Parse(pos_strs[2]);
            transform.position = new Vector3(x, y, z);
            return true;
        }
        catch
        { return false; }
    }

    void Awake()
    {
        controller = GetComponent<Controller>();
        rigidbody = GetComponent<Rigidbody>();
		capsule_collider = GetComponent<CapsuleCollider>();
		
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		frame_yaw = controller.InputValue("Mouse X", true) * yaw_sensitivity;
        frame_pitch = controller.InputValue("Mouse Y", true) * -pitch_sensitivity;
		
        Vector3 run_input = transform.forward * controller.InputValue("Vertical", true);
        run_input += transform.right * controller.InputValue("Horizontal", true);
		run_input.Normalize();
		
		if(grounded)
		{
			if(!Mathf.Approximately(run_input.magnitude, 0))
			{ run_vel = run_input * run_speed; }
			else{ run_vel = Vector3.zero; }
		}
		else
		{ run_vel = run_input * run_speed * air_control; }
		
		if(controller.Pressed(InputCode.JUMP) && grounded)
		{ jump_vel = run_input * jump_thrust.x + Vector3.up * jump_thrust.y; }
	
		if(Input.GetKeyDown(KeyCode.F))
		{
			foreach(SpawnPoint point in FindObjectsOfType<SpawnPoint>())
			{
				point.Orient();
			}
		}
    }

    void FixedUpdate()
    {
		transform.Rotate(Quaternion.AngleAxis(frame_yaw * Time.fixedDeltaTime, Vector3.up).eulerAngles);
		camera.transform.Rotate(Quaternion.AngleAxis(frame_pitch * Time.fixedDeltaTime, Vector3.right).eulerAngles);
		
		CheckGrounding();
		CheckBumping();
		
		if(!grounded)
		{ jump_vel -= Vector3.up * gravity * Time.fixedDeltaTime; }
		else if(!last_grounded)
		{ jump_vel = Vector3.zero; }
		
		if(bumping)
		{
			run_vel = Vector3.zero;
			
			if(!last_bumping)
			{ jump_vel = Vector3.zero; }
		}
		
		rigidbody.MovePosition(transform.position + vel * Time.fixedDeltaTime);
    }
}
