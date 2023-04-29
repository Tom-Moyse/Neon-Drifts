using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftPosts : MonoBehaviour, IDriftObject
{
    [SerializeField]
    private float resetTime, scoreValue, scoreMultiplier;
    private bool available = true;
    private float resetTimer = 0.0f;
    [SerializeField]
    private Material[] statusMaterials;
    private MeshRenderer lightBarRenderer;
    private Vector3 p1pos;
    private Vector3 p2pos;
    void Start(){
        lightBarRenderer = transform.GetChild(2).GetComponent<MeshRenderer>();
        lightBarRenderer.material = statusMaterials[0];

        p1pos = transform.GetChild(0).position + new Vector3(0.0f, 1.0f, 0.0f);
        p2pos = transform.GetChild(1).position + new Vector3(0.0f, 1.0f, 0.0f);
    }
    void Update(){
        if (!available){
            if (resetTimer > resetTime){
                available = true;
                resetTimer = 0.0f;
                lightBarRenderer.material = statusMaterials[0];
            }
            else{
                resetTimer += Time.deltaTime;
            }
        }
    }
    public bool checkZone(BoxCollider box){
        if (!available) { return false; }
        
        return box.Raycast(new Ray(p1pos, p2pos - p1pos), out RaycastHit c, (p2pos - p1pos).magnitude);
    }

    public Vector2 calculateDriftScore(Transform t){
        // Angle of car slide
        float driftAngle = Vector3.Angle(t.forward, t.GetComponent<Rigidbody>().velocity);
        float driftAngleMult = 1 - Mathf.Abs(1 - driftAngle / 90.0f);

        lightBarRenderer.material = statusMaterials[1];
        available = false;
        return new Vector2(scoreValue * driftAngleMult, scoreMultiplier);
    }
}
