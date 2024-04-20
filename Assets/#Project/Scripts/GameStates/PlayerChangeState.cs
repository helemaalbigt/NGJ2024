using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class PlayerChangeState : MonoState {
    
    public MonoState runState;
    public BodyManager bodyManager;
    public VrButton toNext;
    public PageGroup pageGroup;
    public Page page;
    public CheckPointManager checkPointManager;

    private void OnEnable() {
        SceneFinder.I.detector.SetActive(false);
        SceneFinder.I.minesAvailableUI.SetActive(false);
        bodyManager.EnableMineCollision(false);
        pageGroup.OpenPage(page);
        
        MineManager.Instance.ShowVisuals(false);
        checkPointManager.ResetAllCheckpoints();
    }

    void Update()
    {
        if (toNext.interactableState == InteractableState.clicked)
        {
            GoToState(runState);
        }
        
#if UNITY_EDITOR
        if (InputManager.I.PrimaryButtonDown(Hand.right)) {
            GoToState(runState);
        }
#endif
    }
}
