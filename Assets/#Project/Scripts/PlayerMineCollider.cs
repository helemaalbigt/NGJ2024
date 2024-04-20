using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMineCollider : MonoBehaviour
{
    public AudioSource _explosionAudioSource;

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
            other.GetComponentInParent<Mine>().SetState(Mine.State.Triggered);
        }
    }

    private void Die()
    {
        Debug.Log("Die");
        if (_explosionAudioSource)
            _explosionAudioSource.Play();


    }
}
