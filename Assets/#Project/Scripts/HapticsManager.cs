using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using System;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager Instance { get; private set; }

    private Dictionary<Guid, HapticClipPlayer> _players;

    private void Awake()
    {
        Instance = this;
    }

    public Guid PlayHapticClip(HapticClip aClip, bool aIsLooping = false, Controller aController = Controller.Both)
    {
        HapticClipPlayer player = new HapticClipPlayer(aClip);
        player.isLooping = aIsLooping;
        player.Play(aController);
        Guid uid = Guid.NewGuid();
        _players.Add(uid, player);

        return uid;
    }

    public void ChangeAmplitude(Guid aUID, float anAmplitude)
    {
        if (!_players.ContainsKey(aUID))
            return;

        _players[aUID].amplitude = Mathf.Clamp(anAmplitude, 0.0f, 1.0f);
    }

    public void ChangeFrequency(Guid aUID, float aFrequency)
    {
        if (!_players.ContainsKey(aUID))
            return;

        _players[aUID].frequencyShift = Mathf.Clamp(aFrequency, -1.0f, 1.0f);
    }

    public void Stop(Guid aUID)
    {
        if (!_players.ContainsKey(aUID))
            return;

        _players[aUID].Stop();
    }

    public void StopAll()
    {
        foreach (var player in _players)
        {
            player.Value.Stop();
        }

        _players.Clear();
    }
}
