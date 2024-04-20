using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class DetectingState : MonoState {
   public MonoState placingState;
   public GameObject detector;

   private void OnEnable() {
      detector.SetActive(true);
      //MineManager.Instance.ShowVisuals(false);
   }

   private void Update() {

      if (InputManager.I.PrimaryButtonDown(Hand.right)) {
         GoToState(placingState);
      }
   }
}
