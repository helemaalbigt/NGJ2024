using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class GameSetupState : MonoState {
    public Transform startMound;
    public Transform playerHead;

    private void OnEnable() {
        startMound.position = new Vector3(playerHead.position.x, 0, playerHead.position.z);
        startMound.forward = Vector3.ProjectOnPlane(playerHead.forward, Vector3.up);
    }
    
    void Update()
    {
        
    }
}
