using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class PlayerChangeState : MonoState {
    
    public MonoState runState;
    public GameObject detector;
    public BodyManager bodyManager;

    private void OnEnable() {
        detector.SetActive(false);
        bodyManager.EnableMineCollision(false);
    }

    void Update()
    {
        // Skip with button
        if (InputManager.I.PrimaryButtonDown(Hand.right))
        {
            GoToState(runState); // Here it should call GameManager.StartRun(_currentPlayerId)
        }
    }
}
