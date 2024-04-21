using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class ScoreState : MonoState {
    
    public BodyManager bodyManager;
    public VrButton toNext;
    
    public MonoState gameOverState;
    public MonoState roundOverState;
    public MonoState playerChangeState;
    
    public PageGroup pageGroup;
    public Page page;

    private bool _isGameOver;
    private bool _isRoundOver;

    private void OnEnable() {
        SceneFinder.I.detector.SetActive(false);
        SceneFinder.I.minesAvailableUI.SetActive(false);
        bodyManager.EnableMineCollision(false);
        _isGameOver = GameManager.Instance.IsGameOver();
        _isRoundOver = GameManager.Instance.IsRoundOver();
        pageGroup.OpenPage(page);
    }

    void Update()
    {
        if (toNext.interactableState == InteractableState.clicked) {
            GameManager.Instance.SetNextRunState();
            if (_isGameOver)
                GoToState(gameOverState);
            else if (_isRoundOver)
                GoToState(roundOverState);
            else
                GoToState(playerChangeState);
        }
        
#if UNITY_EDITOR
        if (InputManager.I.PrimaryButtonDown(Hand.right)) {
            GameManager.Instance.SetNextRunState();
            if (_isGameOver)
                GoToState(gameOverState);
            else if (_isRoundOver)
                GoToState(roundOverState);
            else
                GoToState(playerChangeState);
        }
#endif
    }
}
