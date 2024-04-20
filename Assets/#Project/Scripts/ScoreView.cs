using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour {
    public Text text;

    private void Update() {
        text.text = "PLAYER " + (GameManager.Instance.GetCurrentPlayerId() + 1).ToString();
    }
}
