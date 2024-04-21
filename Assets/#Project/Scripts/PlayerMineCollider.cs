using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMineCollider : MonoBehaviour
{
    public Image _explosionCameraEffectImage;
    public AnimationCurve _explosionCameraEffectCurve;
    public float _explosionCameraEffectDuration = 2.0f;
    public AudioSource _explosionFeedbackAudioSource;
    public float _explosionFeedbackAudioDelay = 0.0f;

    private bool _isEnabled = false;

    public void Enable(bool aValue)
    {
        _isEnabled = aValue;
    }

    public void OnEnable()
    {
        SetExplosionCameraEffectAlpha(0.0f);
        StartCoroutine(ExplosionFeedback_Coroutine());
    }

    public void OnDisable()
    {
        SetExplosionCameraEffectAlpha(0.0f);
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

        SetExplosionCameraEffectAlpha(0.0f);

        float elapsedTime = 0.0f;
        while (elapsedTime <= _explosionCameraEffectDuration)
        {
            float alpha = _explosionCameraEffectCurve.Evaluate(elapsedTime / _explosionCameraEffectDuration);
            SetExplosionCameraEffectAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SetExplosionCameraEffectAlpha(_explosionCameraEffectCurve.Evaluate(1.0f));

        yield return new WaitForEndOfFrame();

        SetExplosionCameraEffectAlpha(0.0f);
    }

    void SetExplosionCameraEffectAlpha(float alpha)
    {
        // Here you assign a color to the referenced material,
        // changing the color of your renderer
        Color color = _explosionCameraEffectImage.color;
        color.a = Mathf.Clamp(alpha, 0, 1);
        _explosionCameraEffectImage.color = color;

        Debug.Log("A: " + color.a);
    }
}
