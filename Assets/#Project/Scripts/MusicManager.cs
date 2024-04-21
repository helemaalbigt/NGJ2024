using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioSource intro;
    public AudioSource loop;

    public void PlayIntroAndLoop() {
        StartCoroutine(_PlayIntroAndLoop());
        intro.volume = 0.6f;
        loop.volume = 0.6f;
    }

    public void Stop() {
        intro.Stop();
        StartCoroutine(_fadeAndStop());
    }

    private IEnumerator _PlayIntroAndLoop() {
        intro.Play();
        while (intro.isPlaying)
            yield return null;
        loop.Play();
    }

    private IEnumerator _fadeAndStop() {
        var start = Time.unscaledTime;
        var fadeTime = 0.8f;

        var f = 0f;
        while (f < 1.0f) {
            f = Mathf.Clamp01((Time.unscaledTime - start) / fadeTime);
            var vol = Mathf.Lerp(0.6f, 0f, f);
            loop.volume = vol;
            yield return null;
        }
       
        loop.Stop();
    }
}
