using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //variables to get the car to move and do other things
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private OffTrackDetector offTrackDetector; 

    private int terrainLayer;

    private bool offTrack; 
    private OffTrackDetector trackDetector; 
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    private Rigidbody rb;
    public Transform massCenter;
    public GameObject car;

    public float nitroDuration = 2f;
    public float nitroForce = 1000f;
    private bool isNitroAvailable = true;

    public float explosionForce = 1000f;
    public KeyCode selfDestructKey = KeyCode.B;
    public float explosionDelay = 0f;
    public float respawnDelay = 0f;

    //serialized fields to make the variables visible in the inspector
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;


    private void ApplyNitroBoost()
    {
        // Apply nitro boost force to car Rigidbody. Makes the car go faster
        GetComponent<Rigidbody>().AddForce(transform.forward * nitroForce, ForceMode.Acceleration);

        // Set nitro boost on cooldown and waits until it can be used again
        isNitroAvailable = false;
        StartCoroutine(WaitForNitroCooldown());
    }

    //waits for the nitro to be available again
    IEnumerator WaitForNitroCooldown()
    {
        // Wait for nitro duration
        yield return new WaitForSeconds(nitroDuration);
        isNitroAvailable = true;
    }

    //resets the car with an explosion
    void TriggerSelfDestruct()
    {
        // Apply explosion force to car Rigidbody
        GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 10f);

        // Start respawn coroutine
        StartCoroutine(RespawnCoroutine());
    }

    //waits for a period of time before respawning the car
    IEnumerator RespawnCoroutine()
    {
        // Wait for delay
        yield return new WaitForSeconds(respawnDelay);

        // Reset car position and rotation
        transform.position = transform.position;
        transform.rotation = Quaternion.identity;

        // Set velocity and angular velocity to zero to ensure car is right side up
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    //changes the center of mass of the car so that the car can not be flipped
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = offTrackDetecto;
        Debug.Log(rb.centerOfMass);
        offTrackDetector = GetComponentInChildren<OffTrackDetector>(); 
        if (offTrackDetector != null)
        {
            offTrackDetector.OnCarOffTrack += HandleCarOffTrack;
            offTrackDetector.OnCarOnTrack += HandleCarOnTrack;
            offTrack = false;
        }
    }

    //gets the input from the player
    private void GetInput()
    {
        //gets the horizontal and vertical input from the player and stores them in variables
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        //checks to see if a player is pressing the space bar for the break
        isBreaking = Input.GetKey(KeyCode.Space);

        //checks to see if the player is pressing the left shift key and if the nitro is available to apply the nitro boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && isNitroAvailable)
        {
            ApplyNitroBoost();
        }

        //checks to see if the player is pressing the B key to trigger the self destruct
        if (Input.GetKeyDown(selfDestructKey))
        {
            TriggerSelfDestruct();
        }
    }

    //handles the movement of the car
    private void FixedUpdate()
    {
        //calls function to get the input from the player
        GetInput();
        //calls the functions to handle the movement of the car
        HandleMotor();
        //calls the function to handle the steering of the car
        HandleSteering();
        //calls the function to update the wheels
        UpdateWheels();
    }

    //handles the movement of the car
    private void HandleMotor()
    {   
        verticalInput = Input.GetAxis(VERTICAL);
        //sets the motor torque of the wheels to the vertical input times the motor force
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        //checks to see if car is breaking and if it is then sets the break force to the break force variable if not then sets it to 0
        currentbreakForce = isBreaking ? breakForce : 0f;
        //checks to see if the car is breaking and if it is or isn't it then calls the function to apply the break force
        if(isBreaking || offTrack)
        {
            ApplyBreaking();
            rb.drag = 0.75F;
        }
        else{
            ApplyBreaking();
            rb.drag =0.05F;
        }
    }

    //applies the break force to the wheels
    public void ApplyBreaking()
    {
        //sets the break torque of the wheels to the current break force
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;

        //sets the is breaking variable to false
        isBreaking = false;

    }

    //handles the steering of the car
    private void HandleSteering()
    {
        //sets the current steer angle to the max steer angle times the horizontal input
        currentSteerAngle = maxSteerAngle * horizontalInput;
        //assigns front wheels steer angle to current steer angle
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;

    }

    //updates visual representation of the wheels
    private void UpdateWheels()
    {
        //calls the function to update the wheels
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);

    }

    //updates visual representation of a wheel
    private void UpdateSingleWheel(WheelCollider wheelCollider,Transform wheelTransform)
    {
        //variables to store the position and rotation of the wheel
        Vector3 pos;
        Quaternion rot;

        //gets the position and rotation of the wheel and stores them in the variables
        wheelCollider.GetWorldPose(out pos, out rot);
        //sets rotation and position of the wheel to the variables
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void HandleCarOffTrack(){
        offTrack = true;
        Debug.Log(rb.drag); 
    }

    private void HandleCarOnTrack(){
        rb.drag = .2f;
        offTrack = false;
    }

    //unused attempt at calculating the center of mass of the car
    private Vector3 CalculateCenterOfCar()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child != transform) // Exclude the car's root transform
            {
                sum += child.position;
                count++;
            }
        }

        return sum / count;
    }
    public float GetSpeedMPH()
    {
        if (rb == null) return 0f;

        // Convert the car's speed from meters per second to miles per hour
        float speedMPH = rb.velocity.magnitude * 2.23694f;
        return speedMPH;
    }   

}