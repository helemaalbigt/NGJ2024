using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using MyBox;

public class ColorEffect : Effect
{
    public Values values;
    
    [Space(15)]
    public bool updateWhenDisabled = true;
    public Renderer renderer;
    [ConditionalField(nameof(renderer), false)]
    public string renderProp = "_Color";
    [FormerlySerializedAs("textMesh")] public Text text;
    public Image[] images;
    [Space(15)]
    public bool lerp;
    public float delay;
    public float transitionTime = 0.3f;
    private Color _targetColor;

    private void OnEnable() {
        StopAllCoroutines();
        SetColor(_targetColor);
    }

    private void OnDisable() {
        StopAllCoroutines();
        if (updateWhenDisabled) {
            SetColor(_targetColor);
        }
    }
    
    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.disabled:
                ChangeColor(values.disabled);
                break;

            case InteractableState.normal:
                ChangeColor(values.normal);
                break;

            case InteractableState.normalHover:
                ChangeColor(values.hover);
                break;

            case InteractableState.clicked:
                ChangeColor(values.clicked);
                break;

            case InteractableState.clickedHover:
                ChangeColor(values.clickedHover);
                break;
        }
    }

    private void ChangeColor(Color color) {
        if (updateWhenDisabled || enabled) {
            _targetColor = color;
            if (lerp && gameObject.activeInHierarchy) {
                StopAllCoroutines();
                StartCoroutine(_ChangeColor(color));
            } else {
                SetColor(color);
            }
        }
    }
    
    private IEnumerator _ChangeColor(Color color) {
        if (delay > 0) {
            yield return new WaitForSecondsRealtime(delay);
        }

        var startColor = GetColor();
        var startTime = Time.unscaledTime;

        while (Time.unscaledTime - startTime < transitionTime) {
            var f = (Time.unscaledTime - startTime) / transitionTime;
            SetColor(Color.Lerp(startColor, _targetColor, f));
            yield return null;
        }

        SetColor(_targetColor);
    }
    
    private Color GetColor() {
        if (renderer != null) {
            return renderer.material.color;
        }

        if (text != null) {
            return text.color;
        }

        if (images.Length > 0) {
            return images[0].color;
        }

        return Color.black;
    }
    
    private void SetColor(Color color) {
        if (renderer != null) {
            renderer.material.SetColor(renderProp, color);
        }

        if (text != null) {
            text.color = color;
        }

        if (images != null) {
            images.ForEach(i => i.color = color);
        }
    }

    [System.Serializable]
    public struct Values
    {
        public Color disabled;
        public Color normal;
        public Color hover;
        public Color clicked;
        public Color clickedHover;
    }
}
