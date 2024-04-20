using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class LobbyState : MonoState {
   public MonoState checkPointState;
   public PageGroup pageGroup;
   public Page page;
   public MonoState runState;

   [Space(15)] public VrButton checkPointButton;
   public VrButton startButton;
   
   private void OnEnable() {
      SceneFinder.I.detector.SetActive(false);
      pageGroup.OpenPage(page);
   }

   private void Update() {
      if (checkPointButton.interactableState == InteractableState.clicked) {
         GoToState(checkPointState);
      }
      
      if (startButton.interactableState == InteractableState.clicked) {
         GameManager.Instance.StartGame();
         GoToState(runState);
      }
   }
}
