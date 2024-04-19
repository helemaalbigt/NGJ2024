using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using UnityEditor;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager Instance { get; private set; }

    private Dictionary<GUID, HapticClipPlayer> myPlayers;

    private void Awake()
    {
        Instance = this;
    }

    public GUID PlayHapticClip(HapticClip aClip, bool aIsLooping = false, Controller aController = Controller.Both)
    {
        HapticClipPlayer player = new HapticClipPlayer(aClip);
        player.isLooping = aIsLooping;
        player.Play(aController);
        GUID uid = GUID.Generate();
        myPlayers.Add(uid, player);

        return uid;
    }

    public void ChangeAmplitude(GUID aUID, float anAmplitude)
    {
        if (!myPlayers.ContainsKey(aUID))
            return;

        myPlayers[aUID].amplitude = Mathf.Clamp(anAmplitude, 0.0f, 1.0f);
    }

    public void ChangeFrequency(GUID aUID, float aFrequency)
    {
        if (!myPlayers.ContainsKey(aUID))
            return;

        myPlayers[aUID].frequencyShift = Mathf.Clamp(aFrequency, -1.0f, 1.0f);
    }

    public void Stop(GUID aUID)
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
