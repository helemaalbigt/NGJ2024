using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class RoundOverState : MonoState {
    
    public BodyManager bodyManager;
    public VrButton toNext;
    
    public MonoState playerChangeState;
    
    public PageGroup pageGroup;
    public Page page;

    private void OnEnable() {
        SceneFinder.I.detector.SetActive(false);
        SceneFinder.I.minesAvailableUI.SetActive(false);
        bodyManager.EnableMineCollision(false);
        pageGroup.OpenPage(page);
    }

    void Update()
    {
        if (toNext.interactableState == InteractableState.clicked) {
            GoToState(playerChangeState);
        }
        
#if UNITY_EDITOR
        if (InputManager.I.PrimaryButtonDown(Hand.right)) {
            GoToState(playerChangeState);
        }
#endif
    }
}
