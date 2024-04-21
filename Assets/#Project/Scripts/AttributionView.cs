using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributionView : MonoBehaviour {
    public int playerId = 0;
    public int explosions = 0;

    [Space(15)] public Text player;
    public Text stars;
    public Text penalty;
    public Text bonus;
    
    public void Update() {
        var isActivePlayer = playerId == GameManager.Instance.GetCurrentPlayerId();
        var playerStr = (playerId + 1).ToString();
        player.text = isActivePlayer ? "by yourself" : $"by player {playerStr}";
        var starsStr = "";
        for (int i = 0; i < explosions; i++) {
            starsStr += "✴";
        }
        stars.text = starsStr;
        
        float p = isActivePlayer ? ScoreModifiers.SelfHitPenality : ScoreModifiers.HitPenality;
        double t = explosions * p;
        var penaltyTimeSpan = TimeSpan.FromSeconds(t);
        penalty.text = "+" + penaltyTimeSpan.ToString(@"mm\:ss");
        
        bonus.gameObject.SetActive(!isActivePlayer);
        double t2 = explosions * (float)ScoreModifiers.HitBonus;
        var bonusTimeSpan = TimeSpan.FromSeconds(t2);
        bonus.text = "player " + playerStr + ": -"+ bonusTimeSpan.ToString(@"mm\:ss");
    }
}
