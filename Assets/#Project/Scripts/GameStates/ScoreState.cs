using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class ScoreState : MonoState {
    
    public GameObject detector;
    public BodyManager bodyManager;
    public VrButton toNext;
    
    public MonoState gameOverState;
    public MonoState playerChangeState;

    private bool _isGameOver;

    private void OnEnable() {
        detector.SetActive(false);
        bodyManager.EnableMineCollision(false);
        _isGameOver = GameManager.Instance.IsGameOver();
    }

    void Update()
    {
        if (toNext.interactableState == InteractableState.clicked) {
            GameManager.Instance.SetNextRunState();
            GoToState(_isGameOver ? gameOverState : playerChangeState);
        }
    }
}
