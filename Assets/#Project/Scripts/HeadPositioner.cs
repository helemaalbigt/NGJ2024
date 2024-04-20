using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPositioner : MonoBehaviour
{

    void Update() {
        var rot = transform.localRotation;
        rot.eulerAngles = new Vector3(90f, 0, 0);
    }
}
