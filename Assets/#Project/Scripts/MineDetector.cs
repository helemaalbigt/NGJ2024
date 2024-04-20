using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using System;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class MineDetector : MonoBehaviour
{
    public float _innerDetectionRadius = 0.1f;
    public float _maxDetectionDistance = 5.0f;
    public HapticClip _hapticClip;
    public AudioClip _audioClip;
    public float _minFeedbackInterval = 0.2f;
    public float _maxFeedbackInterval = 4.0f;

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

            ChangeFeedbackFrequency(frequency);

            //Debug.Log("Distance: " + distance + " Frequency: " + frequency);
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
            if (!_audioSource.loop)
            {
                _audioSource.loop = true;
                _audioSource.Play();
                _lastFeedbackPlayTimestamp = Time.realtimeSinceStartup;
                if (_hapticClip)
                {
                    _hapticGuid = HapticsManager.Instance.PlayHapticClip(_hapticClip, true, Controller.Right);
                }
            }
        }
        else
        {
            float feedbackInterval = Mathf.Lerp(_minFeedbackInterval, _maxFeedbackInterval, _feedbackFrequency);
            float timeSinceFeedback = Time.realtimeSinceStartup - _lastFeedbackPlayTimestamp;
            //Debug.Log("Feedback Interval: " + feedbackInterval + "Time since: " + timeSinceFeedback);
            if (timeSinceFeedback >= feedbackInterval)
            {
                if (_hapticClip)
                {
                    _hapticGuid = HapticsManager.Instance.PlayHapticClip(_hapticClip, false, Controller.Right);
                }

                if (_audioClip && _audioSource)
                {
                    _audioSource.loop = false;
                    _audioSource.Play();
                }

                _lastFeedbackPlayTimestamp = Time.realtimeSinceStartup;
            }
        }
    }
}
