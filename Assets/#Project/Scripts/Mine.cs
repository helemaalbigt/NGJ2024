using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public List<GameObject> _meshes = new List<GameObject>();
    public GameObject _collider;
    public GameObject _destroyedVisuals;

    public AudioSource _explosionAudioSource;
    public HapticClip _explosionHapticClip;

    public GameObject _innerDebug;
    public GameObject _outerDebug;

    public enum State
    {
        Idle,
        Active,
        Triggered
    }

    [HideInInspector] public State _state;
    [HideInInspector] public int _playerId;
    [HideInInspector] public int _roundCount;

    private void Awake()
    {
        SetState(State.Idle);
    }

    public void Show(bool aValue)
    {
        foreach (GameObject obj in _meshes)
        {
            obj.SetActive(aValue);
        }
    }

    public void SetPlayerIdAndRound(int aPlayerId, int aRoundCount)
    {
        _playerId = aPlayerId;
        _roundCount = aRoundCount;
        // Set mesh color
    }

    public void ShowDebug(float anInner, float anOuter)
    {
        _innerDebug.transform.localScale = new Vector3(anInner, _innerDebug.transform.localScale.y, anInner);
        _outerDebug.transform.localScale = new Vector3(anOuter, _outerDebug.transform.localScale.y, anOuter);

        _innerDebug.SetActive(true);
        _outerDebug.SetActive(true);
    }

    public void HideDebug()
    {
        _innerDebug.SetActive(false);
        _outerDebug.SetActive(false);
    }

    public void SetState(State aState)
    {
        switch (aState)
        {
            case State.Idle:
                _collider.SetActive(false);
                break;
            case State.Active:
                _collider.SetActive(true);
                _destroyedVisuals.SetActive(false);
                break;
            case State.Triggered:
                _collider.SetActive(false);
                _destroyedVisuals.SetActive(true);
                _explosionAudioSource.Play();
                if (_explosionHapticClip)
                    HapticsManager.Instance.PlayHapticClip(_explosionHapticClip);
                HideDebug();
                Show(false);
                break;
        }

        _state = aState;
    }
}
