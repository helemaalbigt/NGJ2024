using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using System;

public class MineDetector : MonoBehaviour
{
    public float _innerDetectionRadius = 0.1f;
    public float _maxDetectionDistance = 5.0f;
    public HapticClip _hapticClip;
    public AudioClip _audioClip;

    private Guid _hapticGuid = Guid.Empty;
    private AudioSource _audioSource = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.clip = _audioClip;
    }

    private void Update()
    {
        Vector3 closestMinePosition = Vector3.zero;
        if (MineManager.Instance.GetClosestMinePosition(transform.position, out closestMinePosition))
        {
            float distance = Vector3.Distance(closestMinePosition, transform.position);
            if (distance <= _innerDetectionRadius)
                ChangeFeedbackFrequency(1.0f);
            else if (distance > _maxDetectionDistance)
                ChangeFeedbackFrequency(0.0f);
            else
                ChangeFeedbackFrequency(Mathf.InverseLerp(_innerDetectionRadius, _maxDetectionDistance, distance));
        }
    }

    private void ChangeFeedbackFrequency(float aFrequency)
    {
        if (_hapticClip)
        {
            if (_hapticGuid == Guid.Empty && aFrequency != 0.0f)
            {
                _hapticGuid = HapticsManager.Instance.PlayHapticClip(_hapticClip, true, Controller.Right);
            }

            if (_hapticGuid != Guid.Empty)
            {
                if (aFrequency == 0.0f)
                {
                    HapticsManager.Instance.Stop(_hapticGuid);
                    _hapticGuid = Guid.Empty;
                }
                else
                    HapticsManager.Instance.ChangeFrequency(_hapticGuid, aFrequency);
            }
        }

        if (_audioClip && _audioSource)
        {
            if (aFrequency == 0.0f)
                _audioSource.Stop();
            else
            {
                _audioSource.pitch = aFrequency;
                _audioSource.Play();
            }
        }
    }
}
