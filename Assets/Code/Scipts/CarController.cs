using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float springStrength, damperStrength, maxSuspensionDistance, restSuspensionDistance,
        gripStrength, rollingFriction, topSpeed, engineTorque, brakingForce, tireMass, steerAngle,
        minDriftVelocity;

    [HideInInspector]
    public bool isDrifting;
    private GameObject wheels;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        wheels = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
        /*
        Debug.Log(rb.position.ToString());
        Debug.Log(wheels.transform.GetChild(0).position.ToString());
        Debug.Log(wheels.transform.GetChild(1).position.ToString());
        Debug.Log(wheels.transform.GetChild(2).position.ToString());
        Debug.Log(wheels.transform.GetChild(3).position.ToString());
        */
    }

    void FixedUpdate()
    {
        Transform wheel;
        // Loop through each wheel
        for (int i = 0; i < 4; i++){
            wheel = wheels.transform.GetChild(i);

            // Check if car in suspension range thus on ground
            RaycastHit intersect;
            Ray down = new Ray(wheel.position, -wheel.up);

            if (Physics.Raycast(down, out intersect, maxSuspensionDistance)){
                // Calculate suspension force
                float wheelVel = Vector3.Dot(rb.GetPointVelocity(wheel.position), wheel.up);
                float offset = restSuspensionDistance - intersect.distance;

                float force = (offset * springStrength) - (wheelVel * damperStrength);

                rb.AddForceAtPosition(force * wheel.up, wheel.position);
                //rb.AddForceAtPosition(new Vector3(0, force, 0), wheel.position);
                //Debug.Log("Tire " + i + "Force: " + (force * wheel.up).ToString());

                // Calculate drive forces
                // Slip/steering
                float slipVel = Vector3.Dot(rb.GetPointVelocity(wheel.position), wheel.right);
                float slipChange = -slipVel * gripStrength;
                float slipAccel = slipChange / Time.fixedDeltaTime;

                rb.AddForceAtPosition(wheel.right * tireMass * slipAccel, wheel.position);
                //Debug.Log("Tire " + i + "Force: " + (wheel.right * rb.mass * slipAccel).ToString());
                //Debug.DrawLine(wheel.position, wheel.position + (wheel.right * rb.mass * slipAccel), Color.yellow);


                // Acceleration
                float speed = Vector3.Dot(transform.forward, rb.velocity);
                float speedPercent = Mathf.Clamp01(Mathf.Abs(speed) / topSpeed);
                if (Input.GetAxis("Vertical") > 0.0f){
                    float torque = (1 - speedPercent) * engineTorque * Input.GetAxis("Vertical");
                    rb.AddForceAtPosition(wheel.forward * torque, wheel.position);
                }
                // Braking
                else if (Input.GetAxis("Vertical") < 0.0f){
                    if (speed > -10.0f){
                        rb.AddForceAtPosition(-brakingForce * wheel.forward, wheel.position);   
                    }
                    // else if (speed < 0.0f){
                    //     rb.AddForceAtPosition(brakingForce * wheel.forward, wheel.position);   
                    // } 
                }
                else{
                    // Rolling friction
                    rb.AddForceAtPosition(-speed * gripStrength * tireMass * rollingFriction * wheel.forward, wheel.position);
                }
                
            }
        }

        // Apply steering
        float steerInput = Input.GetAxis("Horizontal");
        float steerRotate = steerInput * steerAngle;

        wheels.transform.GetChild(2).localRotation = Quaternion.Euler(0.0f, steerRotate, 0.0f);
        wheels.transform.GetChild(3).localRotation = Quaternion.Euler(0.0f, steerRotate, 0.0f);

        // Calculate if in drifting state
        isDrifting = Mathf.Abs(Vector3.Dot(transform.right, rb.velocity)) > minDriftVelocity ||
                       (90 - Mathf.Abs(90 - Vector3.Angle(transform.forward, rb.velocity)) > steerAngle && rb.velocity.magnitude > 5.0f);
        //Debug.Log("Drift vel: " + Mathf.Abs(Vector3.Dot(transform.right, rb.velocity)) + "Drift angle: " + (90 - Mathf.Abs(90 - Vector3.Angle(transform.forward, rb.velocity))));
    }
}
