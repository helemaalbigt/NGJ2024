using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreView : MonoBehaviour {
    public Text winnerName;
    public Text winnerTime;

    public Transform parent;
    public LoserScoreView loserPrefab;

    private List<GameObject> losers = new List<GameObject>(); //lol

    private void OnEnable() {
        List<KeyValuePair<int, float>> leaderBoard = GameManager.Instance.GetOrderedLeaderBoard();

        winnerName.text = "PLAYER " + (leaderBoard[0].Key + 1);
        var span = TimeSpan.FromSeconds(leaderBoard[0].Value);
        winnerTime.text = span.ToString(@"mm\:ss");

        for (int i = 1; i < leaderBoard.Count; i++) {
            var kv = leaderBoard[i];
            var loser = Instantiate(loserPrefab, parent);
            losers.Add(loser.gameObject);
            loser.id = kv.Key;
            loser.time = kv.Value;
        }
    }

    private void OnDisable() {
        foreach (var l in losers) {
            Destroy(l);
        }
        losers.Clear();
    }
}
