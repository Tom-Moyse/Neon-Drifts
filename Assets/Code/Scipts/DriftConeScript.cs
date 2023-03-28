using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftConeScript : MonoBehaviour, IDriftObject
{
    public float zoneRadius = 5.0f;
    private float lastDistance;
    [SerializeField]
    private float resetTime;
    [SerializeField]
    private int maxScoreCalls;
    private bool available = true;
    private int currentScoreCalls = 0;
    private float resetTimer = 0.0f;
    private Light statusLight;
    void Start(){
        statusLight = transform.GetChild(0).GetComponent<Light>();
        statusLight.color = Color.green;
    }
    void Update(){
        if (!available){
            if (resetTimer > resetTime){
                available = true;
                resetTimer = 0.0f;
                statusLight.color = Color.green;
            }
            else{
                Debug.Log(resetTimer);
                resetTimer += Time.deltaTime;
            }
        }
    }
    public bool checkZone(BoxCollider box){
        if (!available) { return false; }
        Vector3 close = box.ClosestPointOnBounds(transform.position);
        float distance = (close - transform.position).magnitude;
        lastDistance = distance;

        return (distance < zoneRadius);
    }

    public float calculateDriftScore(Transform t){
        if (++currentScoreCalls >= maxScoreCalls){
            available = false;
            currentScoreCalls = 0;
            statusLight.color = Color.cyan;
            return 0.0f;
        }
        // Calculate score parameters
        float distMult = 1 - (lastDistance/zoneRadius);
        // Angle of car slide
        float driftAngle = Vector3.Angle(t.forward, t.GetComponent<Rigidbody>().velocity);
        float driftAngleMult = 1 - Mathf.Abs(1 - driftAngle / 90.0f);
        // Angle relative to object
        float objectAngle = Vector3.Angle(t.forward, transform.position - t.position);
        float objectAngleMult = Mathf.Abs(1 - objectAngle / 90.0f);

        //Debug.Log(distMult + ", " + driftAngleMult + ", " + objectAngleMult);
        return distMult * driftAngleMult * objectAngleMult;
    }
}
