using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMound : MonoBehaviour
{
    public AudioSource reachedAudioSource;

    public void OnReached()
    {
        if (reachedAudioSource && reachedAudioSource.clip)
            reachedAudioSource.Play();
    }
}
