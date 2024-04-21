using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundViewBig : MonoBehaviour {
    public Text text;
    void Update() {
        text.text = $"ROUND {GameManager.Instance.GetCurrentRound() + 1}";
    }
}
