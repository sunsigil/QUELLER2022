using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour, ISavable
{
    [Header("Motion")]

    [SerializeField]
    float speed;

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

    float pitch;
    float yaw;
    Vector3 motion_dir;

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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        pitch -= controller.InputValue("Mouse Y", true) * pitch_sensitivity * Time.deltaTime;
        yaw += controller.InputValue("Mouse X", true) * yaw_sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -pitch_range, pitch_range);
        camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        transform.localEulerAngles = new Vector3(0, yaw, 0);

        motion_dir = Vector3.zero;
        motion_dir += transform.forward * controller.InputValue("Vertical", true);
        motion_dir += transform.right * controller.InputValue("Horizontal", true);
        motion_dir.Normalize();
    }

    void FixedUpdate()
    {
        Vector3 motion = motion_dir * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + motion);
    }
}
