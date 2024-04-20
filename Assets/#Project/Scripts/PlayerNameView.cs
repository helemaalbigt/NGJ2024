using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameView : MonoBehaviour {
   public Text text;

   void Update() {
      text.text = "PLAYER " + (GameManager.Instance.GetCurrentPlayerId() + 1).ToString();
   }
}
