using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool steering;
    public bool power;

    public float steerAngle { get; set; }
    public float torque { get; set; }

    private WheelCollider wheelCollider;
    private Transform wheelTransform;

    // Start is called before the first frame update
    void Start()
    {
        wheelCollider = GetComponentInChildren<WheelCollider>();
        wheelTransform = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelsRotation();
    }

    private void FixedUpdate()
    {
        if (steering)
        {
            wheelCollider.steerAngle = steerAngle * 1;
        }

        if (power)
        {
            wheelCollider.motorTorque = torque * 1;
        }
    }

    private void WheelsRotation()
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot * Quaternion.Euler(new Vector3(0, -90, 0));
    }
}
