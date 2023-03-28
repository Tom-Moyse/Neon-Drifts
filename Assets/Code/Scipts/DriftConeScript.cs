using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftConeScript : MonoBehaviour, IDriftObject
{
    public float zoneRadius = 5.0f;
    private float lastDistance;

    void Start(){

    }
    public bool checkZone(BoxCollider box){
        Vector3 close = box.ClosestPointOnBounds(transform.position);
        float distance = (close - transform.position).magnitude;
        lastDistance = distance;

        return (distance < zoneRadius);
    }

    public float calculateDriftScore(Transform t){
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
