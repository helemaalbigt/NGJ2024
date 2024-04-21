using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour {
    public Text activePlayer;
    public Text playerTime;
    public GameObject penaltyWrapper;
    public Text penaltyText;

    [Space(25)]
    public GameObject[] trippedMinesWrapper;
    public Text trippedMines;
    public Transform parent;
    public AttributionView playerAttribution;

    [Space(25)] public AudioSource chime;
    public AudioSource boom;

    private int _minesTotal = 0;
    private float _penaltyTotal = 0f;
    private List<GameObject> attributions = new List<GameObject>();

    private void OnEnable() {
        attributions.Clear();
        _minesTotal = 0;
        _penaltyTotal = 0f;
        
        StartCoroutine(ShowScores());
    }

    private IEnumerator ShowScores() {
        GameManager.Run run = GameManager.Instance.GetCurrentRun();
        playerTime.gameObject.SetActive(false);
        penaltyWrapper.SetActive(false);
        trippedMinesWrapper.ForEach(g => g.SetActive(false));
        activePlayer.text = "PLAYER " + (GameManager.Instance.GetCurrentPlayerId() + 1);
        trippedMines.text = "0";

        yield return new WaitForSeconds(0.4f);
        
        playerTime.gameObject.SetActive(true);
        var span = TimeSpan.FromSeconds(run.time);
        playerTime.text = span.ToString(@"mm\:ss");
        chime.Play();
        
        yield return new WaitForSeconds(1f);

        var minesTriggeredPerPlayer = new int[GameManager.Instance.playerCount];
        for (int i = 0; i < minesTriggeredPerPlayer.Length; i++) {
            minesTriggeredPerPlayer[i] = 0;
        }
        foreach (var mine in run.minesTriggered) {
            minesTriggeredPerPlayer[mine._playerId]++;
        }
        
        for (int i = 0; i < minesTriggeredPerPlayer.Length; i++) {
            var m = minesTriggeredPerPlayer[i];
            if (m > 0) {
                var attr = Instantiate(playerAttribution, parent);
                attributions.Add(attr.gameObject);
                attr.playerId = i;
                for (int j = 0; j < m; j++) {
                    yield return new WaitForSeconds(0.5f);
                    attr.explosions++;
                    boom.Play();
                    
                    penaltyWrapper.SetActive(true);
                    _minesTotal++;
                    trippedMines.text = _minesTotal.ToString();
                    var isActivePlayer = i == GameManager.Instance.GetCurrentPlayerId();
                    float extraPenalty = isActivePlayer ? ScoreModifiers.SelfHitPenality : ScoreModifiers.HitPenality;
                    _penaltyTotal += extraPenalty;
                    var penSpan = TimeSpan.FromSeconds(_penaltyTotal);
                    penaltyText.text = "penalty: +" + penSpan.ToString(@"mm\:ss");
                }
            }
        }
    }

    private void OnDisable() {
        foreach (var at in attributions) {
            Destroy(at);
        }
        attributions.Clear();
        _minesTotal = 0;
        _penaltyTotal = 0f;
    }
}
