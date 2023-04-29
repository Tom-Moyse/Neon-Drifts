using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DriftScorer : MonoBehaviour
{
    private BoxCollider bc;
    private List<IDriftObject> DriftObjects = new List<IDriftObject>();
    private float score = 0.0f;
    private float scoreMultiplier = 1.0f;
    private float driftResetTimer = 0.0f;
    private float levelTimer;
    

    public TMPro.TMP_Text textInfo;

    public GameController gc;
    [SerializeField]
    private float driftResetTime, levelTime;
    void Start(){
        bc = GetComponent<BoxCollider>();
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < allScripts.Length; i++)
        {
            if(allScripts[i] is IDriftObject)
                DriftObjects.Add(allScripts[i] as IDriftObject);
        }
        levelTimer = levelTime;
    }
    void FixedUpdate(){
        if (bc.gameObject.GetComponent<CarController>().isDrifting){
            bool touchedDriftObject = false;
            foreach (IDriftObject driftObject in DriftObjects)
            {
                if (driftObject.checkZone(bc)){
                    Vector2 scoreInfo = driftObject.calculateDriftScore(transform);
                    score += scoreMultiplier * scoreInfo[0];
                    scoreMultiplier += scoreInfo[1];
                    touchedDriftObject = true;
                    break;
                }

            }

            // Default drift score calculation
            if (!touchedDriftObject){
                float driftAngle = Vector3.Angle(transform.forward, GetComponent<Rigidbody>().velocity);
                score += 1 * scoreMultiplier * (1 - Mathf.Abs(1 - driftAngle / 90.0f));
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

        levelTimer -= Time.fixedDeltaTime;

        textInfo.text = "Score: " + score.ToString("F0") + " x" + scoreMultiplier.ToString("F2");
        textInfo.text += "\nTime: " + levelTimer.ToString("F0");

        if (levelTimer <= 0){
            Time.timeScale = 0;
            gc.showSummary((int) score);
        }
    }

    private void OnCollisionEnter(Collision collision){
        Debug.Log("Ya kinda bad");
        scoreMultiplier = 1.0f;
    }
}
