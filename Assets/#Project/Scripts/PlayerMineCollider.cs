using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMineCollider : MonoBehaviour
{
    public OVRPassthroughLayer _passthroughLayer;
    public AnimationCurve _explosionCameraEffectCurve;
    public float _explosionCameraEffectDuration = 1.0f;
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
            SufferDamage();
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

    private void SufferDamage()
    {
        StartCoroutine(ExplosionFeedback_Coroutine());
    }

    private IEnumerator ExplosionFeedback_Coroutine()
    {
        if (_explosionFeedbackAudioSource)
            _explosionFeedbackAudioSource.PlayDelayed(_explosionFeedbackAudioDelay);

        yield return new WaitForEndOfFrame();

        _passthroughLayer.colorMapEditorType = OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment;

        float elapsedTime = 0.0f;
        while (elapsedTime <= _explosionCameraEffectDuration)
        {
            _passthroughLayer.colorMapEditorBrightness = _explosionCameraEffectCurve.Evaluate(elapsedTime / _explosionCameraEffectDuration);
            elapsedTime += Time.deltaTime;
        }
        _passthroughLayer.colorMapEditorBrightness = _explosionCameraEffectCurve.Evaluate(1.0f);

        yield return new WaitForEndOfFrame();

        _passthroughLayer.colorMapEditorBrightness = 0.0f;
        _passthroughLayer.colorMapEditorType = OVRPassthroughLayer.ColorMapEditorType.None;
    }
}
