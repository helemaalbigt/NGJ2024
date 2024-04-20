using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class RunState : MonoState
{
   public BodyManager bodyManager;
   public MonoState scoreState;
   private Mine _placingMine;
   
   private void OnEnable() {
      GameManager.Instance.StartRun();
      SceneFinder.I.detector.SetActive(true);
      bodyManager.EnableMineCollision(true);
   }
   
   void Update()
   {
      if (InputManager.I.TriggerDown(Hand.left)) {
         var spawnPos = GetSpawnPos();
         _placingMine = GameManager.Instance.PlaceMine(spawnPos);
      }

      if (_placingMine != null) {
         _placingMine.transform.position =  GetSpawnPos();
         if (InputManager.I.TriggerUp(Hand.left)) {
            _placingMine = null;
         }
      }

      if (GameManager.Instance.runState == GameManager.RunState.Ended) {
         GoToState(scoreState);
      }
   }

   private Vector3 GetSpawnPos() {
      return new Vector3(SceneFinder.I.leftHand.position.x, 0, SceneFinder.I.leftHand.position.z);
   }
}
