using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class ScoreState : MonoState {
    
    public MonoState playerChangeState;
    public GameObject detector;
    public BodyManager bodyManager;

    private void OnEnable() {
        detector.SetActive(false);
        bodyManager.EnableMineCollision(false);
    }

    void Update()
    {
        // Show the score
        // Skip with button
        if (InputManager.I.PrimaryButtonDown(Hand.right))
        {
            GoToState(GameManager.Instance.GetEndRunState());
        }
    }
}
