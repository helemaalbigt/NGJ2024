using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class PlayerChangeState : MonoState {
    
    public MonoState runState;
    public GameObject detector;
    public BodyManager bodyManager;
    public VrButton toNext;

    private void OnEnable() {
        detector.SetActive(false);
        bodyManager.EnableMineCollision(false);
    }

    void Update()
    {
        if (toNext.interactableState == InteractableState.clicked)
        {
            GoToState(runState);
        }
    }
}
