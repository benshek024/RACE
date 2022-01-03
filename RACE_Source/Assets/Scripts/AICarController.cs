using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("Axis Strings")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    [Header("Wheel Colliders")]
    public WheelCollider wheelColliderLeftFront;
    public WheelCollider wheelColliderLeftBack;
    public WheelCollider wheelColliderRightFront;
    public WheelCollider wheelColliderRightBack;

    [Header("Wheel's Transform")]
    public Transform wheelLeftFront;
    public Transform wheelLeftBack;
    public Transform wheelRightFront;
    public Transform wheelRightBack;

    [Header("Car's Speed and Steering Angle")]
    [Tooltip("Car's Engine Power")]
    public float motorTorque = 50f;
    [Tooltip("Car's Maximum Steering Angle")]
    public float maxSteer = 40f;
    public float currentSpeed;
    [Tooltip("Car's Maximum Speed")]
    public float maxSpeed = 60f;

    [Header("Rigidbody and Center Mass")]
    private Rigidbody _rigidBody;
    public Transform carCenterMass;

    [Header("Car's SFX")]
    private float pitch = 0.3f;
    public AudioSource carEngine;
    public ParticleSystem exhaustParticle;
    public Transform target;
    public float maxVolume = 0.3f;

    [Header("AI Path")]
    [Tooltip("Assign Game Object with AIPath script")]
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;

    // Start is called before the first frame update
    void Start()
    {
        carEngine = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = carCenterMass.localPosition;

        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    private void Update()
    {
        WheelsRotation();
        ExhaustSmoke();
        EnginePitch();

        if (carEngine.pitch < 0.3f)
        {
            carEngine.pitch = 0.3f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.startGame)
        {
            Steer();
            Drive();
            CheckDistance();
        }
    }

    private void EnginePitch()
    {
        pitch = currentSpeed / maxSpeed;
        carEngine.pitch = pitch;
    }

    private void ExhaustSmoke()
    {
        var emission = exhaustParticle.emission;

        if (currentSpeed <= 0)
        {
            emission.rateOverTime = 2;
        }
        else
        {
            emission.rateOverTime = 200;
        }
    }

    // Makes AI's car steering to follow current node's position
    void Steer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;
        wheelColliderLeftFront.steerAngle = newSteer;
        wheelColliderRightFront.steerAngle = newSteer;
    }

    // Controls of AI's car
    void Drive()
    {
        currentSpeed = 2 * 22 / 7 * wheelColliderLeftFront.radius * wheelColliderLeftFront.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);

        if (currentSpeed < maxSpeed)
        {
            wheelColliderLeftBack.motorTorque = motorTorque;
            wheelColliderRightBack.motorTorque = motorTorque;
        }
        else
        {
            wheelColliderLeftBack.motorTorque = 0f;
            wheelColliderRightBack.motorTorque = 0f;
        }
    }

    // Rotate the wheels
    private void WheelsRotation()
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        wheelColliderLeftFront.GetWorldPose(out pos, out rot);
        wheelLeftFront.position = pos;
        wheelLeftFront.rotation = rot * Quaternion.Euler(new Vector3(0, -90, 0));

        wheelColliderRightFront.GetWorldPose(out pos, out rot);
        wheelRightFront.position = pos;
        wheelRightFront.rotation = rot * Quaternion.Euler(new Vector3(0, 90, 0));

        wheelColliderLeftBack.GetWorldPose(out pos, out rot);
        wheelLeftBack.position = pos;
        wheelLeftBack.rotation = rot * Quaternion.Euler(new Vector3(0, -90, 0));

        wheelColliderRightBack.GetWorldPose(out pos, out rot);
        wheelRightBack.position = pos;
        wheelRightBack.rotation = rot * Quaternion.Euler(new Vector3(0, 90, 0));
    }

    // Check the distance to current node, 
    //if the distance is less than 10f, 
    //change the current node to next node.
    
    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 10f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AIGoal")
        {
            LapsManager.instance.aiCurrentGoal++;
        }
    }
}
