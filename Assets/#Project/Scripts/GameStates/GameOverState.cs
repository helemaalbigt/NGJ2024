using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class GameOverState : MonoState {
    
    public MonoState lobbyState;
    public GameObject detector;
    public BodyManager bodyManager;
    public VrButton toNext;
    public PageGroup pageGroup;
    public Page page;

    private void OnEnable() {
        detector.SetActive(false);
        bodyManager.EnableMineCollision(false);
        GameManager.Instance.OnGameOver();
        pageGroup.OpenPage(page);
    }

    private void Update() {
        if (toNext.interactableState == InteractableState.clicked) {
            GoToState(lobbyState);
        }
        
#if UNITY_EDITOR
        if (InputManager.I.PrimaryButtonDown(Hand.right)) {
            GoToState(lobbyState);
        }
#endif
    }
}
