using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMineCollider : MonoBehaviour
{
    public AudioSource _explosionFeedbackAudioSource;
    public float _explosionFeedbackAudioDelay = 0.0f;

    private bool _isEnabled = false;

    public void Enable(bool aValue)
    {
        _isEnabled = aValue;
    }

    public void SetPosition(Vector3 aPosition)
    {
        transform.position = aPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isEnabled)
            return;

        if (other.gameObject.layer == 6) // Mine = 6
        {
            Die();
            GameManager.Instance.OnMineTriggered(other.GetComponentInParent<Mine>());
        }
        else if (other.gameObject.layer == 7) // CheckPoint = 7
        {
            GameManager.Instance.OnCheckPointEntered(other.GetComponentInParent<CheckPoint>());
        }
        else if (other.gameObject.layer == 9) // StartMound = 9
        {
            GameManager.Instance.OnStartMoundEnter();
        }
    }

    private void Die()
    {
        Debug.Log("Die");
        if (_explosionFeedbackAudioSource)
            _explosionFeedbackAudioSource.PlayDelayed(_explosionFeedbackAudioDelay);
    }
}
