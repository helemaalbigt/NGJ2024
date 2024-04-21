using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour {
   public int index;
   public bool isActive;
   public bool isChecked;
   public bool placing;
   public bool isValidPos;
   public PlayState playstate = PlayState.setup; 
   
   [Space(25)]
   public Text debugText;
   public Renderer flag;
   public Renderer[] placingRenderers;
   public Material checkedMat;
   public Material unCheckedMat;
   public Material activeMat;
   public Material placingValidMat;
   public Material placingInvalidMat;
   public GameObject placingWrapper;
   public GameObject realWrapper;
   public Transform flagPole;
   
   private void Update() {
      debugText.text = (index + 1).ToString();
      
      placingWrapper.SetActive(placing);
      realWrapper.SetActive(!placing);

      foreach (var rend in placingRenderers) {
         rend.material = isValidPos ? placingValidMat : placingInvalidMat;
      }

      if(isActive) {
         flag.material = activeMat;
      } else if (isChecked) {
         flag.material = checkedMat;
      } else {
         flag.material = unCheckedMat;
      }

      var poleTargetHeight = 0f;
      if (playstate == PlayState.other) {
         poleTargetHeight = -1.65f;
      } else if (!isActive && playstate != PlayState.setup) {
         poleTargetHeight = -0.8f;
      }

      var newpos = flagPole.position;
      newpos.y = poleTargetHeight;
      flagPole.position = Vector3.Lerp(flagPole.position, newpos, Time.unscaledDeltaTime * 5f);
      
   }

   public enum PlayState {
      setup,
      other,
      playing
   }
}
