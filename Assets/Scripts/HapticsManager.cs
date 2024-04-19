using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using UnityEditor;
using System;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager Instance { get; private set; }

    private Dictionary<Guid, HapticClipPlayer> myPlayers;

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
        myPlayers.Add(uid, player);

        return uid;
    }

    public void ChangeAmplitude(Guid aUID, float anAmplitude)
    {
        if (!myPlayers.ContainsKey(aUID))
            return;

        myPlayers[aUID].amplitude = Mathf.Clamp(anAmplitude, 0.0f, 1.0f);
    }

    public void ChangeFrequency(Guid aUID, float aFrequency)
    {
        if (!myPlayers.ContainsKey(aUID))
            return;

        myPlayers[aUID].frequencyShift = Mathf.Clamp(aFrequency, -1.0f, 1.0f);
    }

    public void Stop(Guid aUID)
    {
        if (!myPlayers.ContainsKey(aUID))
            return;

        myPlayers[aUID].Stop();
    }

    public void StopAll()
    {
        foreach (var player in myPlayers)
        {
            player.Value.Stop();
        }

        myPlayers.Clear();
    }
}
