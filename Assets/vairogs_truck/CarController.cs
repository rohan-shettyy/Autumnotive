using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rb;
    Transform tf;

    public float maxSpeed = 10;
    public float turnRate = 1;
    public float gasAcceleration = 50;
    public float brakeAcceleration = 25;
    
    float throttle;
    float steering;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Physics goes here
    void FixedUpdate()
    {
        // TODO: Bind accelerate, decelerate, steer to WASD keys
        accelerate();

        // TODO: Manually add friction force for sideways sliding (drifting)
        Vector3 velocity = rb.velocity;
        Vector3 vPerp = Vector3.Dot(transform.right, velocity)*transform.right;

        float driftTime = 10f; // fixed update iterations

        rb.AddForce(-vPerp/driftTime, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        inputKeyboard();
    }

    //grab control input
    void inputKeyboard()
    {
        throttle = Input.GetAxisRaw("Vertical");
        steering = Input.GetAxisRaw("Horizontal");
    }

    void accelerate() 
    {
        if (rb.velocity.magnitude >= maxSpeed) return;
        if (throttle > 0)
        {
            rb.AddForce(transform.forward*throttle*gasAcceleration, ForceMode.Acceleration);
            
        }
        else if (throttle < 0)
        {
            rb.AddForce(transform.forward*throttle*brakeAcceleration, ForceMode.Acceleration);
        }
    }

    void steer()
    {
        float speed = rb.velocity.magnitude;
        float angularVelocity = 10 * maxSpeed / (speed + 1);
        if (steering > 0) {
            // right
            rb.AddTorque(-transform.up*angularVelocity, ForceMode.VelocityChange);
        } else if (steering < 0) {
            // left
            rb.AddTorque(-transform.up*angularVelocity, ForceMode.VelocityChange);
        } else {
            // no steer
            rb.AddTorque(Vector3.zero, ForceMode.VelocityChange);
        }
    }
}
