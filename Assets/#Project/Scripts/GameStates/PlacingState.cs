using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class PlacingState : MonoState {
    
    public MonoState detectingState;
    public MineManager mineManager;
    public Transform rightHand;
    public GameObject detector;

    private Mine _placingMine;

    private void OnEnable() {
        detector.SetActive(false);
    }

    void Update()
    {
        if (InputManager.I.TriggerDown(Hand.right)) {
            var spawnPos = new Vector3(rightHand.position.x, 0, rightHand.position.z);
            _placingMine = mineManager.Spawn(spawnPos);
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
