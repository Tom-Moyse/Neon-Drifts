using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftCone : MonoBehaviour, IDriftObject
{
    private float zoneRadius = 5.0f;
    private float lastDistance;
    [SerializeField]
    private float resetTime, scoreValue, scoreMultiplier;
    [SerializeField]
    private int maxScoreCalls;
    private bool available = true;
    private int currentScoreCalls = 0;
    private float resetTimer = 0.0f;
    [SerializeField]
    private Material[] statusMaterials;
    private MeshRenderer lightDiscRenderer;
    void Start(){
        lightDiscRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        lightDiscRenderer.material = statusMaterials[0];
    }
    void Update(){
        if (!available){
            if (resetTimer > resetTime){
                available = true;
                resetTimer = 0.0f;
                lightDiscRenderer.material = statusMaterials[0];
            }
            else{
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

    public Vector2 calculateDriftScore(Transform t){
        if (++currentScoreCalls >= maxScoreCalls){
            available = false;
            currentScoreCalls = 0;
            lightDiscRenderer.material = statusMaterials[1];
            return Vector2.zero;
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
        return new Vector2(distMult * driftAngleMult * objectAngleMult, scoreMultiplier);
    }
}
