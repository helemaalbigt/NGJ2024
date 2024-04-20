using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using System;
using Unity.VisualScripting;

public class MineDetector : MonoBehaviour
{
    public float _innerDetectionRadius = 0.1f;
    public float _maxDetectionDistance = 5.0f;
    public HapticClip _hapticClip;
    public AudioClip _audioClip;
    public float _minFeedbackInterval = 0.1f;
    public float _maxFeedbackInterval = 2.0f;

    private Guid _hapticGuid = Guid.Empty;
    private AudioSource _audioSource = null;
    private float _lastFeedbackPlayTimestamp;
    private float _feedbackFrequency;

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
            float frequency = 0.0f;
            if (distance <= _innerDetectionRadius)
                frequency = 1.0f;
            else if (distance > _maxDetectionDistance)
                frequency = 0.0f;
            else
                frequency = Mathf.InverseLerp(_innerDetectionRadius, _maxDetectionDistance, distance);

            if (frequency != _feedbackFrequency)
                ChangeFeedbackFrequency(frequency);
        }
        else
        {
            ChangeFeedbackFrequency(0.0f);
        }
    }

    private void ChangeFeedbackFrequency(float aFrequency)
    {
        _feedbackFrequency = aFrequency;

        if (aFrequency == 0.0f)
        {
            if (_hapticGuid != Guid.Empty)
            {
                HapticsManager.Instance.Stop(_hapticGuid);
                _hapticGuid = Guid.Empty;
            }

            if (_audioClip && _audioSource)
            {
                _audioSource.Stop();
            }

            _lastFeedbackPlayTimestamp = 0;
        }
        else if (aFrequency == 1.0f)
        {
            _audioSource.loop = true;
            _audioSource.Play();
            _lastFeedbackPlayTimestamp = Time.realtimeSinceStartup;
            if (_hapticClip)
            {
                _hapticGuid = HapticsManager.Instance.PlayHapticClip(_hapticClip, true, Controller.Right);
            }
        }
        else
        {
            float feedbackInterval = Mathf.InverseLerp(_feedbackFrequency, _minFeedbackInterval, _maxFeedbackInterval);
            if (Time.realtimeSinceStartup - _lastFeedbackPlayTimestamp >= feedbackInterval)
            {
                if (_hapticClip)
                {
                    _hapticGuid = HapticsManager.Instance.PlayHapticClip(_hapticClip, false, Controller.Right);
                }

                if (_audioClip && _audioSource)
                {
                    _audioSource.Play();
                }
            }
        }
    }
}
