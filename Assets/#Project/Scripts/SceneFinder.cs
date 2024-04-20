using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFinder : MonoBehaviour {
    public static SceneFinder I;
    
    // Start is called before the first frame update
    void Awake() {
        if(I == null)
            I = this;
    }

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public GameObject detector;
}
