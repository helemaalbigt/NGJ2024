using System;
using UnityEngine;

namespace Rowhouse
{
    public abstract class Effect : MonoBehaviour
    {
        public Interactable interactable;
        
        private bool _oscilate;
        private float _phase = 2f;
        private InteractableState _fromState = InteractableState.disabled;
        private InteractableState _toState = InteractableState.normal;
        private InteractableState _current;
        private float _timeSinceLastChange = 0f;

        public virtual void Start() {
            interactable.onButtonStateChange += OnStateChange;
            Render();
        }

        public virtual void Update() {
            if (_oscilate && !interactable.isHovered()) {
                if (_timeSinceLastChange > _phase / 2f) {
                    _current = _current == _fromState ? _toState : _fromState;
                    OnStateChange(_current);
                    _timeSinceLastChange = _timeSinceLastChange - (_phase / 2f);
                }
                
                _timeSinceLastChange += Time.unscaledDeltaTime;
            }
        }

        public void SetOscilating(bool enabled, float phase = 2f, InteractableState fromState = InteractableState.disabled, InteractableState toState = InteractableState.normal) {
            _oscilate = enabled;
            _phase = phase;
            _fromState = fromState;
            _toState = toState;
            
            if (enabled) {
                _current = fromState;
                OnStateChange(fromState);
            } else {
                Render();
            }
        }

        public void Render() {
            OnStateChange(interactable.interactableState);
        }

        protected abstract void OnStateChange(InteractableState state);
    }
}