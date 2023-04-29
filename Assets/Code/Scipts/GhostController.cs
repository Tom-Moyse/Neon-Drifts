using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{  
    private ReplayKeyframe[] keys;
    private float startTime;
    int prevIndex;
    int nextIndex;
    public void Initialize(ReplayKeyframe[] ks){
        keys = ks;
        startTime = Time.time;
        prevIndex = 0;
        nextIndex = 1;
    }

    public void Update(){
        float elapsedTime = Time.time - startTime;

        while (elapsedTime > keys[nextIndex].relativeTimestamp){
            prevIndex++;
            if (++nextIndex >= keys.Length){
                Destroy(gameObject);
            }
        }

        Vector3 prevPos = new Vector3(keys[prevIndex].position[0], keys[prevIndex].position[1], keys[prevIndex].position[2]);
        Vector3 nextPos = new Vector3(keys[nextIndex].position[0], keys[nextIndex].position[1], keys[nextIndex].position[2]);

        Quaternion prevRot = new Quaternion(keys[prevIndex].rotation[0], keys[prevIndex].rotation[1],
                                            keys[prevIndex].rotation[2], keys[prevIndex].rotation[3]);
        Quaternion nextRot = new Quaternion(keys[nextIndex].rotation[0], keys[nextIndex].rotation[1],
                                            keys[nextIndex].rotation[2], keys[nextIndex].rotation[3]);
        
        float interpolant = (elapsedTime - keys[prevIndex].relativeTimestamp) / 
                            (keys[nextIndex].relativeTimestamp - keys[prevIndex].relativeTimestamp);
        
        Vector3 newPos = Vector3.Lerp(prevPos, nextPos, interpolant);
        Quaternion newRot = Quaternion.Slerp(prevRot, nextRot, interpolant);

        transform.position = newPos;
        transform.rotation = newRot;
    }
}
