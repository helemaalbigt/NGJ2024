using System;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class OverlapSphereInteractor : MonoBehaviour
{
    public Transform castCenterHt;
    public Transform castCenterControllers;
    public float radius = 0.01f;
    public LayerMask interactableLayers;

    public Hand _hand;

    [Header("Debug")]
    [SerializeField]
    private Collider[] _hoveringColliders = new Collider[10];
    [SerializeField]
    private InteractionListener _hoveringListener;
    private bool _hoveringListenerChangedthisFrame;
    [SerializeField]
    private List<InteractionListener> _hoveringListeners = new();

    private const float HoverSwitchMargin = 0.01f;

    private void Awake() {
        if (castCenterHt == null) {
            castCenterHt = transform;
        }

        if (castCenterControllers == null) {
            castCenterControllers = transform;
        }
    }

    public bool IsHovering() {
        return _hoveringListener != null; // _hoveringListeners.Count > 0;
    }

    public string GetHoveringTag() {
        return _hoveringListener == null ? "" : _hoveringListener.gameObject.tag;
    }

    private void LateUpdate() {
        Array.Clear(_hoveringColliders, 0, _hoveringColliders.Length);

        var center = castCenterControllers;
        Physics.OverlapSphereNonAlloc(center.position, radius, _hoveringColliders, interactableLayers);

        _hoveringListeners.Clear();
        _hoveringListenerChangedthisFrame = false;

        foreach (var collider in _hoveringColliders) {
            if (collider != null) {
                var listener = collider.GetComponent<InteractionListener>();
                if (listener != null) {
                    _hoveringListeners.Add(listener);
                }
            }
        }

        //the hovering listener may have been turned off suddenly and replaced with another button, e.g. when switching between two modals
        if (_hoveringListener != null && !_hoveringListeners.Contains(_hoveringListener)) {
            _hoveringListener.HoverOff(GetData());
            _hoveringListener = null;
        }

        if (_hoveringListeners.Count == 0 && _hoveringListener != null) {
            _hoveringListener.HoverOff(GetData());
            _hoveringListener = null;
        } else if (_hoveringListeners.Count == 1 && _hoveringListener == null) {
            _hoveringListener = _hoveringListeners[0];
            _hoveringListener.HoverOn(GetData());
        } else {
            foreach (var listener in _hoveringListeners) {
                if (_hoveringListener == null || listener == _hoveringListener) {
                    continue;
                }

                var distToNewListener = Vector3.Distance(center.position, listener.interactionCenter.position);
                var distToOldListener = Vector3.Distance(center.position, _hoveringListener.interactionCenter.position);

                if (distToNewListener + HoverSwitchMargin < distToOldListener) {
                    _hoveringListener.HoverOff(GetData());
                    _hoveringListenerChangedthisFrame = true;
                    _hoveringListener = listener;
                }
            }

            if (_hoveringListenerChangedthisFrame) {
                _hoveringListener.HoverOn(GetData());
            }
        }
    }

    private void OnDisable() {
        HoverOffAll();
    }

    private void HoverOffAll() {
        if (IsHovering()) {
            _hoveringListener.HoverOff(GetData());
        }
        _hoveringListener = null;
        _hoveringListeners.Clear();
    }

    private InteractionData GetData() {
        return new InteractionData {
            interactorGameObject = gameObject,
            interactionTransform = transform,
            interactorType = typeof(OverlapSphereInteractor),
            hand = _hand
        };
    }
}