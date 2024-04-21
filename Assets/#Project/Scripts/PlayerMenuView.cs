using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuView : MonoBehaviour {
    public Text text;

    public VrButton increase;
    public VrButton decrease;
    // Start is called before the first frame update
    void Start() {
        increase.OnClick += IncreaseOnOnClickUp;
        decrease.OnClick += DecreaseOnOnClickUp;
    }

    private void DecreaseOnOnClickUp() {
        var current = GameManager.Instance.playerCount;
        GameManager.Instance.playerCount = Mathf.Clamp(current - 1, 2, 99);
    }

    private void IncreaseOnOnClickUp() {
        var current = GameManager.Instance.playerCount;
        GameManager.Instance.playerCount = Mathf.Clamp(current + 1, 2, 99);
    }

    // Update is called once per frame
    void Update() {
        text.text = GameManager.Instance.playerCount + " players";

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            GameManager.Instance.playerCount = 3;
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            GameManager.Instance.playerCount = 4;
        }
    }
}
