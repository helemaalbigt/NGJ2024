using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPositioner : MonoBehaviour
{
    void Update() {
        var targetRot = transform.parent.rotation;
        if (transform.parent.rotation.eulerAngles.x > 10f && transform.parent.rotation.eulerAngles.x < 90f) {
            targetRot = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        } 
        transform.rotation = Quaternion.Lerp( transform.rotation, targetRot, Time.unscaledDeltaTime * 5f);
    }
}
