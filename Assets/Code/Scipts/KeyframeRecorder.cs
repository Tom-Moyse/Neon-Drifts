using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframeRecorder : MonoBehaviour
{
    [SerializeField]
    private GameObject car;
    private List<ReplayKeyframe> keyframes;
    private float startTime;

    public IEnumerator recordKeyframes(){
        startTime = Time.time;
        keyframes = new List<ReplayKeyframe>();
        ReplayKeyframe store;

        while (true){
            store = new ReplayKeyframe(car.transform.position.x, car.transform.position.y, 
                                        car.transform.position.z, car.transform.rotation.x, 
                                        car.transform.rotation.y, car.transform.rotation.z, 
                                        car.transform.rotation.w, Time.time - startTime);
            keyframes.Add(store);
            yield return new WaitForSeconds(.1f);
        }
    }

    public ReplayKeyframe[] getKeyframes(){
        return keyframes.ToArray();
    }
}
