using UnityEngine;

public interface IDriftObject{
    bool checkZone(BoxCollider box);
    Vector2 calculateDriftScore(Transform t);
}