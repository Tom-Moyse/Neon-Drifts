using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftScorer : MonoBehaviour
{
    private BoxCollider bc;
    private List<IDriftObject> DriftObjects = new List<IDriftObject>();
    private float score = 0.0f;
    private float scoreMultiplier = 1.0f;
    private float driftResetTimer = 0.0f;

    public TMPro.TMP_Text textScore;
    [SerializeField]
    private float driftResetTime;
    void Start(){
        bc = GetComponent<BoxCollider>();
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < allScripts.Length; i++)
        {
            if(allScripts[i] is IDriftObject)
                DriftObjects.Add(allScripts[i] as IDriftObject);
        }
    }
    void FixedUpdate(){
        if (bc.gameObject.GetComponent<CarController>().isDrifting){
            foreach (IDriftObject driftObject in DriftObjects)
            {
                if (driftObject.checkZone(bc)){
                    score += 10 * scoreMultiplier * driftObject.calculateDriftScore(transform);
                    scoreMultiplier += 0.01f;
                }

            }

            driftResetTimer = 0.0f;
        }

        else{
            driftResetTimer += Time.fixedDeltaTime;
            if (driftResetTimer > driftResetTime){
                driftResetTimer = 0.0f;
                scoreMultiplier = 1.0f;
            }
        }

        textScore.text = "Score: " + score.ToString("F2") + " x" + scoreMultiplier.ToString("F2");
    }

    private void OnCollisionEnter(Collision collision){
        Debug.Log("Ya kinda bad");
        scoreMultiplier = 1.0f;
    }
}
