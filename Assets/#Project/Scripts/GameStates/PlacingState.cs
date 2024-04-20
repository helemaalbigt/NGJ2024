using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class PlacingState : MonoState {
    
    public MonoState detectingState;
    public Transform rightHand;
    public GameObject detector;
    public BodyManager bodyManager;

    private Mine _placingMine;

    private void OnEnable() {
        detector.SetActive(false);
        MineManager.Instance.ShowVisuals(true);
        bodyManager.EnableMineCollision(false);
    }

    void Update()
    {
        if (InputManager.I.TriggerDown(Hand.right)) {
            var spawnPos = new Vector3(rightHand.position.x, 0, rightHand.position.z);
            _placingMine = MineManager.Instance.Spawn(spawnPos, 0);
        }

        if (_placingMine != null) {
            _placingMine.transform.position =  new Vector3(rightHand.position.x, 0, rightHand.position.z);
            if (InputManager.I.TriggerUp(Hand.right)) {
                _placingMine = null;
            }
        }

        if (InputManager.I.PrimaryButtonDown(Hand.right)) {
            GoToState(detectingState);
        }
    }
}
