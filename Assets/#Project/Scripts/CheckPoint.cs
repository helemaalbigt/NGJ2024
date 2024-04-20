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
   }
}
