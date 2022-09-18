using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    Rigidbody rb;
    
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float downForce = 10f;
    [SerializeField] private float maxSpeed =25;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -downForce, 0);

    }
    private void FixedUpdate()
    {
        
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

    }

private void HandleMotor()
    {

        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
        currentBreakForce = 0f;
        //Applies a brake force when the desired direction and velocity direction are opposite.
        if (((localVel.z > 0.2)&&(verticalInput < 0)) || ((localVel.z < -0.2)&&(verticalInput > 0))){
            currentBreakForce = breakForce;
        } 

        //This allows for easy change of direction when colliding with an object
        if (((localVel.z <0.2) && (localVel.z>-0.2))&&(verticalInput != 0)){
            frontLeftWheelCollider.motorTorque  = 0;
            frontRightWheelCollider.motorTorque = 0;
        }
        //car moving forward, slow down:    (localVel.z > 0.2)&&(verticalInput < 0)=50
        //car moving backwards, slow down:  (localVel.z < -0.2)&&(verticalInput > 0)=50
        //car moving forwards, speed up:    (localVel.z > 0.2)&&(verticalInput > 0) 
        //car moving backwards, speed up:   (localVel.z < -0.2)&&(verticalInput < 0)

        //Limits max speed;
        if (rb.velocity.magnitude > maxSpeed) return;


        frontLeftWheelCollider.motorTorque  = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;


        

        ApplyBreaking();
       
    }

    private void ApplyBreaking(){
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering(){
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;

    }
    private void UpdateWheels(){
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform){
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void GetInput(){
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);

    }
}

// public class CarController : MonoBehaviour
// {
//     Rigidbody rb;
//     Transform tf;

//     public float maxSpeed = 10;
//     public float turnRate = 1;
//     public float gasAcceleration = 50;
//     public float brakeAcceleration = 25;
    
//     float throttle;
//     float steering;
//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     // Physics goes here
//     void FixedUpdate()
//     {
//         // TODO: Bind accelerate, decelerate, steer to WASD keys
//         accelerate();

//         // TODO: Manually add friction force for sideways sliding (drifting)
//         Vector3 velocity = rb.velocity;
//         Vector3 vPerp = Vector3.Dot(transform.right, velocity)*transform.right;

//         float driftTime = 10f; // fixed update iterations

//         rb.AddForce(-vPerp/driftTime, ForceMode.VelocityChange);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         inputKeyboard();
//     }

//     //grab control input
//     void inputKeyboard()
//     {
//         throttle = Input.GetAxisRaw("Vertical");
//         steering = Input.GetAxisRaw("Horizontal");
//     }

//     void accelerate() 
//     {
//         if (rb.velocity.magnitude >= maxSpeed) return;
//         if (throttle > 0)
//         {
//             rb.AddForce(transform.forward*throttle*gasAcceleration, ForceMode.Acceleration);
            
//         }
//         else if (throttle < 0)
//         {
//             rb.AddForce(transform.forward*throttle*brakeAcceleration, ForceMode.Acceleration);
//         }
//     }

//     void steer()
//     {
//         float speed = rb.velocity.magnitude;
//         float angularVelocity = 10 * maxSpeed / (speed + 1);
//         if (steering > 0) {
//             // right
//             rb.AddTorque(-transform.up*angularVelocity, ForceMode.VelocityChange);
//         } else if (steering < 0) {
//             // left
//             rb.AddTorque(-transform.up*angularVelocity, ForceMode.VelocityChange);
//         } else {
//             // no steer
//             rb.AddTorque(Vector3.zero, ForceMode.VelocityChange);
//         }
//     }
// }
