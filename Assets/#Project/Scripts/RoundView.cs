using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundView : MonoBehaviour {
    public Text text;
    void Update() {
        text.text = $"-Round {GameManager.Instance.GetCurrentRound() + 1}-";
    }
}
