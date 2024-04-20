using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class DetectingState : MonoState {
   public MonoState placingState;
   public BodyManager bodyManager;

   private void OnEnable() {
      bodyManager.EnableMineCollision(true);
      SceneFinder.I.detector.SetActive(true);
      SceneFinder.I.minesAvailableUI.SetActive(true);
    }

    private void Update() {

      if (InputManager.I.PrimaryButtonDown(Hand.right)) {
         GoToState(placingState);
      }
   }
}
