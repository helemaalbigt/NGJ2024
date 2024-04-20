using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Rowhouse;

public class TranslateEffect : Effect
{
    public Vector3 disabled;
    public Vector3 normal;
    public Vector3 hover;
    public Vector3 clicked;
    public Vector3 clickedHover;

    [Space(25)]
    public Transform targetTransform;
    public float delay = 0f;
    public float transitionTime = 0.3f;
    private Vector3 _targetPos;

    public override void Start() {
        _targetPos = normal;
        targetTransform.localPosition = _targetPos;

        base.Start();
    }

    private void OnDisable() {
        StopAllCoroutines();
        targetTransform.localPosition = _targetPos;
    }

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.disabled:
                MoveTo(disabled);
                break;

            case InteractableState.normal:
                MoveTo(normal);
                break;

            case InteractableState.normalHover:
                MoveTo(hover);
                break;

            case InteractableState.clicked:
                MoveTo(clicked);
                break;

            case InteractableState.clickedHover:
                MoveTo(clickedHover);
                break;
        }
    }

    private void MoveTo(Vector3 pos) {
        if (_targetPos != pos && gameObject.activeInHierarchy) {
            StopAllCoroutines();
            StartCoroutine(_MoveTo(pos));
        } else {
            targetTransform.localPosition = _targetPos;
        }
    }

    IEnumerator _MoveTo(Vector3 pos) {
        if (delay > 0)
            yield return new WaitForSecondsRealtime(delay);

        _targetPos = pos;
        var startPos = targetTransform.localPosition;
        var startTime = Time.unscaledTime;

        while (Time.unscaledTime - startTime < transitionTime) {
            var f = (Time.unscaledTime - startTime) / transitionTime;
            targetTransform.localPosition = Vector3.Lerp(startPos, _targetPos, f);
            yield return null;
        }

        targetTransform.localPosition = _targetPos;
    }
}
