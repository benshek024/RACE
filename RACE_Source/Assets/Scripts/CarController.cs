using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
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

    [Header("Car's Controlling Setting")]
    [Tooltip("Car's Engine Power")]
    public float motorTorque = 400f;
    [Tooltip("Car's Maximum Steering Angle")]
    public float maxSteer = 40f;
    [Tooltip("Car's Braking Power")]
    private float brakes = 400;
    [Tooltip("Car's Maximum Speed")]
    public float maxSpeed = 60f;
    public float currentSpeed = 0f;

    [Header("Rigidbody and Center Mass")]
    private Rigidbody _rigidBody;
    public Transform carCenterMass;

    [Header("Car's SFX")]
    private float pitch = 0.3f;
    public AudioSource carEngine;
    public ParticleSystem exhaustParticle;

    private Timer timer;

    private void Start()
    {
        timer = GameObject.FindObjectOfType<Timer>().GetComponent<Timer>();
        carEngine = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = carCenterMass.localPosition;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.startGame == true)
        {
            Drive();
        }
    }

    private void Update()
    {
        ExhaustSmoke();
        EnginePitch();
        if (GameManager.instance.startGame == true)
        {
            WheelsRotation();
            Brakes();
        }

        // Makes the pitch of engine sound will no go below than 0.3f
        if (carEngine.pitch < 0.3f)
        {
            carEngine.pitch = 0.3f;
        }
    }

    private void Drive()
    {
        // Convert currentSpeed to Miles per hour (mph)
        currentSpeed = 2 * 22 / 7 * wheelColliderLeftFront.radius * wheelColliderLeftFront.rpm * 60 / 1000;
        // Round currentSpeed to nearest integer
        currentSpeed = Mathf.Round(currentSpeed);

        // Applies power to motorTorque while currentSpeed is not exceeding maxSpeed
        if (currentSpeed < maxSpeed)
        {
            wheelColliderLeftBack.motorTorque = Input.GetAxis(verticalAxis) * motorTorque;
            wheelColliderRightBack.motorTorque = Input.GetAxis(verticalAxis) * motorTorque;

            wheelColliderLeftBack.motorTorque = SimpleInput.GetAxis(verticalAxis) * motorTorque;
            wheelColliderRightBack.motorTorque = SimpleInput.GetAxis(verticalAxis) * motorTorque;
        }
        else
        {
            wheelColliderLeftBack.motorTorque = 0f;
            wheelColliderRightBack.motorTorque = 0f;
        }

        // Steer the wheels to turn the car
        wheelColliderLeftFront.steerAngle = Input.GetAxis(horizontalAxis) * maxSteer;
        wheelColliderRightFront.steerAngle = Input.GetAxis(horizontalAxis) * maxSteer;

        wheelColliderLeftFront.steerAngle = SimpleInput.GetAxis(horizontalAxis) * maxSteer;
        wheelColliderRightFront.steerAngle = SimpleInput.GetAxis(horizontalAxis) * maxSteer;
    }

    // Controls the pitch of engine sound
    private void EnginePitch()
    {
        pitch = currentSpeed / maxSpeed;
        carEngine.pitch = pitch;
    }

    // Change the emission rate of the exhaust smoke depends on the speed of the car
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

    // Applies brakes to brakeTorque when pressed SpaceBar or Brake button
    private void Brakes()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            wheelColliderLeftFront.brakeTorque = brakes;
            wheelColliderRightFront.brakeTorque = brakes;
            wheelColliderLeftBack.brakeTorque = brakes;
            wheelColliderRightBack.brakeTorque = brakes;
        }
        else
        {
            wheelColliderLeftFront.brakeTorque = 0;
            wheelColliderRightFront.brakeTorque = 0;
            wheelColliderLeftBack.brakeTorque = 0;
            wheelColliderRightBack.brakeTorque = 0;
        }

        if (SimpleInput.GetButton("Brake"))
        {
            wheelColliderLeftFront.brakeTorque = brakes;
            wheelColliderRightFront.brakeTorque = brakes;
            wheelColliderLeftBack.brakeTorque = brakes;
            wheelColliderRightBack.brakeTorque = brakes;
        }
        else
        {
            wheelColliderLeftFront.brakeTorque = 0;
            wheelColliderRightFront.brakeTorque = 0;
            wheelColliderLeftBack.brakeTorque = 0;
            wheelColliderRightBack.brakeTorque = 0;
        }
    }

    // Rotate the wheels by according the data of wheel collider
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LapsCP")
        {
            other.gameObject.SetActive(false);
            LapsManager.instance.currentCP++;
        }

        if (other.tag == "LapsGoal")
        {
            if (LapsManager.instance.currentCP == LapsManager.instance.totalCP)
            {
                timer.EndLap();
                LapsManager.instance.currentLaps++;
                LapsManager.instance.ResetLaps();
            }

            if (LapsManager.instance.currentLaps == 1 || LapsManager.instance.currentCP == LapsManager.instance.totalCP)
            {
                timer.StartLap();
            }

            if (LapsManager.instance.currentLaps <= LapsManager.instance.totalLaps)
            {
                GameManager.instance.audioSource.PlayOneShot(GameManager.instance.lapPassedClip, 0.3f);
            }
        }
    }
}
