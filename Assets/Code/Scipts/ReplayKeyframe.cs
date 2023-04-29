using System.Collections;
using System.Collections.Generic;

public class ReplayKeyframe
{
    public float[] position;
    public float[] rotation;
    public float relativeTimestamp;

    public ReplayKeyframe(float xp, float yp, float zp, float xq, float yq, float zq, float wq, float time){
        position = new float[3] {xp, yp, zp};
        rotation = new float[4] {xq, yq, zq, wq};
        relativeTimestamp = time;
    }
}
