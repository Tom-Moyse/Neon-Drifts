using UnityEngine;

public interface IDriftObject{
    bool checkZone(BoxCollider box);
    float calculateDriftScore(Transform t);
}