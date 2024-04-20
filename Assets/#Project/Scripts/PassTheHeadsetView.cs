using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassTheHeadsetView : MonoBehaviour {
    public Text text;
    void Update() {
        text.text = $"Pass the headset to PLAYER {GameManager.Instance.GetCurrentPlayerId() + 1}";
    }
}
