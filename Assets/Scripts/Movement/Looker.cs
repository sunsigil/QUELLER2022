using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    [SerializeField]
    float pitch_range;
    [SerializeField]
    float pitch_sensitivity;
    [SerializeField]
    float yaw_sensitivity;

    [SerializeField]
    Transform cam_anchor;
    [SerializeField]
    Transform weapon_anchor;

	Controller controller;

    float frame_yaw;
    float frame_pitch;

    void Awake()
    {
		controller = GetComponent<Controller>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update()
	{
		frame_yaw = controller.InputValue("Mouse X", true) * yaw_sensitivity;
		frame_pitch = controller.InputValue("Mouse Y", true) * -pitch_sensitivity;
	}

    private void FixedUpdate()
    {
        Quaternion yaw_quat = Quaternion.AngleAxis(frame_yaw * Time.fixedDeltaTime, Vector3.up);
        Quaternion pitch_quat = Quaternion.AngleAxis(frame_pitch * Time.fixedDeltaTime, Vector3.right);

        transform.Rotate(yaw_quat.eulerAngles);
        cam_anchor.transform.Rotate(pitch_quat.eulerAngles);
        weapon_anchor.transform.Rotate(pitch_quat.eulerAngles);
    }
}
