using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update() {
        var fwd = Vector3.ProjectOnPlane(transform.position - SceneFinder.I.head.position, Vector3.up);
        transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
    }
}
