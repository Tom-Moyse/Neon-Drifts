using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float springStrength;
    public float damperStrength;
    public float maxSuspensionDistance;
    public float restSuspensionDistance;
    public float gripStrength;
    public float topSpeed;
    public float engineTorque;
    public float brakingForce;
    public float tireMass;
    public float steerAngle;
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
                if (Input.GetAxis("Vertical") > 0.0f){
                    float speed = Vector3.Dot(transform.forward, rb.velocity);
                    float speedPercent = Mathf.Clamp01(Mathf.Abs(speed) / topSpeed);
                    float torque = (1 - speedPercent) * engineTorque * Input.GetAxis("Vertical");
                    rb.AddForceAtPosition(wheel.forward * torque, wheel.position);
                }
                // Braking
                else if (Input.GetAxis("Vertical") < 0.0f){
                    float speed = Vector3.Dot(transform.forward, rb.velocity);
                    if (speed > 0.0f){
                        rb.AddForceAtPosition(-brakingForce * wheel.forward, wheel.position);
                    }           
                }
            }
        }

        // Apply steering
        float steerInput = Input.GetAxis("Horizontal");
        float steerRotate = steerInput * steerAngle;

        wheels.transform.GetChild(2).localRotation = Quaternion.Euler(0.0f, steerRotate, 0.0f);
        wheels.transform.GetChild(3).localRotation = Quaternion.Euler(0.0f, steerRotate, 0.0f);
    }
}
