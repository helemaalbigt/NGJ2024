using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class GameOverState : MonoState {
    
    public MonoState runState;
    public GameObject detector;
    public BodyManager bodyManager;

    private void OnEnable() {
        detector.SetActive(false);
        bodyManager.EnableMineCollision(false);
        GameManager.Instance.OnGameOver();
    }
}
