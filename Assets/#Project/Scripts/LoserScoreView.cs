using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoserScoreView : MonoBehaviour {
    public int id;
    public float time;
    
    public Text nameText;
    public Text timeText;

    // Update is called once per frame
    void Update()
    {
        nameText.text = "PLAYER " + (id + 1).ToString();
        var span = TimeSpan.FromSeconds(time);
        timeText.text = span.ToString(@"mm\:ss");
    }
}
