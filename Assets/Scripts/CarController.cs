using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private const int SOLID_OBJECTS_LAYER = -1;

    [HideInInspector] public float speed;
    [HideInInspector] public float speedMax = 25f;
    private float speedMin = -25f;
    private float acceleration = 15f;
    private float brakeSpeed = 50f;
    private float reverseSpeed = 10f;
    private float idleSlowdown = 5f;

    private float turnSpeed;
    private float turnSpeedMax = 300;
    private float turnSpeedAcceleration = 300;
    private float turnIdleSlowdown = 500;

    private float forwardAmount;
    private float turnAmount;

    private Rigidbody carRigidbody;
    private Car car;

    [SerializeField] private Transform Transform_Front_Left;
    [SerializeField] private Transform Transform_Front_Right;
    [SerializeField] private Transform Transform_Back_Left;
    [SerializeField] private Transform Transform_Back_Right;

    [SerializeField] private WheelCollider Collider_Front_Left;
    [SerializeField] private WheelCollider Collider_Front_Right;
    [SerializeField] private WheelCollider Collider_Back_Left;
    [SerializeField] private WheelCollider Collider_Back_Right;

    [SerializeField] private float Force;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float brakeForce;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    bool isBraking;
    private float currentBrakeForce;
    private float currentSteerAngle;

    public bool isRun = false;

    public float hoverHeight = 1f; // Максимальное расстояние рейкаста
    public LayerMask terrainLayer;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        car = GetComponent<Car>();
    }

    private void FixedUpdate()
    {
        //GetIntput();
        //HandleMotor();
        //HandleSteering();
        //UpateWheels();

        if (!isRun)
            return;

        float deltaTime = Time.fixedDeltaTime;

        if (forwardAmount > 0)
        {
            // Accelerating
            speed += forwardAmount * acceleration * deltaTime;
        }
        if (forwardAmount < 0)
        {
            if (speed > 0)
            {
                // Braking
                speed += forwardAmount * brakeSpeed * deltaTime;
            }
            else
            {
                // Reversing
                speed += forwardAmount * reverseSpeed * deltaTime;
            }
        }

        if (forwardAmount == 0)
        {
            // Not accelerating or braking
            if (speed > 0)
            {
                speed -= idleSlowdown * deltaTime;
            }
            if (speed < 0)
            {
                speed += idleSlowdown * deltaTime;
            }
        }

        speed = Mathf.Clamp(speed, speedMin, speedMax);

        carRigidbody.velocity = transform.forward * speed;
        //carRigidbody.velocity += -transform.up * 9.81f;

        if (speed < 0)
        {
            // Going backwards, invert wheels
            turnAmount = turnAmount * -1f;
        }

        if (turnAmount > 0 || turnAmount < 0)
        {
            // Turning
            if ((turnSpeed > 0 && turnAmount < 0) || (turnSpeed < 0 && turnAmount > 0))
            {
                // Changing turn direction
                float minTurnAmount = 20f;
                turnSpeed = turnAmount * minTurnAmount;
            }
            turnSpeed += turnAmount * turnSpeedAcceleration * deltaTime;
        }
        else
        {
            // Not turning
            if (turnSpeed > 0)
            {
                turnSpeed -= turnIdleSlowdown * deltaTime;
            }
            if (turnSpeed < 0)
            {
                turnSpeed += turnIdleSlowdown * deltaTime;
            }
            if (turnSpeed > -1f && turnSpeed < +1f)
            {
                // Stop rotating
                turnSpeed = 0f;
            }
        }

        float speedNormalized = speed / speedMax;
        float invertSpeedNormalized = Mathf.Clamp(1 - speedNormalized, .75f, 1f);

        turnSpeed = Mathf.Clamp(turnSpeed, -turnSpeedMax, turnSpeedMax);

        carRigidbody.angularVelocity = new Vector3(0, turnSpeed * (invertSpeedNormalized * 1f) * Mathf.Deg2Rad, 0);

        if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        AlignToSurface();
    }

    void AlignToSurface()
    {
        RaycastHit hit;

        // Пускаем луч вниз от объекта
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, terrainLayer))
        {
            // Расстояние до земли и нормаль к поверхности
            float distanceToGround = hit.distance;
            Vector3 groundNormal = hit.normal;

            // Поддержание постоянного расстояния до земли
            Vector3 hoverPosition = transform.position;
            hoverPosition.y += (hoverHeight - distanceToGround);
            transform.position = hoverPosition;

            // Поворот в соответствии с наклоном поверхности
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = targetRotation/*Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5)*/;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SOLID_OBJECTS_LAYER != -1)
        {
            if (collision.gameObject.layer == SOLID_OBJECTS_LAYER)
            {
                speed = Mathf.Clamp(speed, 0f, 20f);
            }
        }
    }

    public void SetInputs(float forwardAmount, float turnAmount)
    {
        this.forwardAmount = forwardAmount;
        this.turnAmount = turnAmount;
    }

    public void ClearTurnSpeed()
    {
        turnSpeed = 0f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeedMax(float speedMax)
    {
        this.speedMax = speedMax;
    }

    public void SetTurnSpeedMax(float turnSpeedMax)
    {
        this.turnSpeedMax = turnSpeedMax;
    }

    public void SetTurnSpeedAcceleration(float turnSpeedAcceleration)
    {
        this.turnSpeedAcceleration = turnSpeedAcceleration;
    }

    public void StopCompletely()
    {
        speed = 0f;
        turnSpeed = 0f;
    }

    public Vector3 GetRigidbodyVelocity()
    {
        return carRigidbody.velocity;
    }

/*    private void GetIntput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        Collider_Front_Left.motorTorque = verticalInput * Force;
        Collider_Front_Right.motorTorque = verticalInput * Force;
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        Collider_Front_Left.brakeTorque = currentBrakeForce;
        Collider_Front_Right.brakeTorque = currentBrakeForce;
        Collider_Back_Left.brakeTorque = currentBrakeForce;
        Collider_Back_Right.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        //currentSteerAngle = maxSteerAngle * horizontalInput;
        Collider_Front_Left.steerAngle = currentSteerAngle;
        Collider_Front_Right.steerAngle = currentSteerAngle;
    }

    private void UpateWheels()
    {
        UpateSingleWheel(Collider_Front_Left, Transform_Front_Left);
        UpateSingleWheel(Collider_Front_Right, Transform_Front_Right);
        UpateSingleWheel(Collider_Back_Left, Transform_Back_Left);
        UpateSingleWheel(Collider_Back_Right, Transform_Back_Right);
    }

    private void UpateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void StopCompletely()
    {
        verticalInput = 0;
        currentSteerAngle = 0;
        currentBrakeForce = 0;
    }*/
}
