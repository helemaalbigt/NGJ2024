using System;
using UnityEngine;

namespace Rowhouse
{
    //Base class for interactable elements
    public abstract class Interactable : MonoBehaviour
    {
        public event Action<InteractableState> onButtonStateChange;

        [SerializeField]
        protected bool _active = true;
        public bool active {
            get => _active;
            set {
                if (_active != value) {
                    _active = value;
                    if (!_active) {
                        interactableState = InteractableState.disabled;
                        cursorInteractions.ForceRemoveInteractions();
                    } else {
                        interactableState = InteractableState.normal;
                    }
                }
            }
        }

        public InteractionListener cursorInteractions;

        public static Interactable focusedInteractable;

        private void Awake() {
            if (cursorInteractions == null) {
                cursorInteractions = GetComponentInChildren<InteractionListener>();
            }
        }

        public bool isHovered() {
            return _interactableState == InteractableState.normalHover || _interactableState == InteractableState.clickedHover;
        }

        [Header("Debug")]
        [SerializeField]
        private InteractableState _interactableState = InteractableState.normal;
        public InteractableState interactableState {
            get => _interactableState;
            set {
                if (value != _interactableState) {
                    _interactableState = value;
                    onButtonStateChange?.Invoke(_interactableState);

                    if (value == InteractableState.clicked && _interactableState != InteractableState.clickedHover) {
                        focusedInteractable = this;
                    }
                }
            }
        }
    }

    public enum InteractableState
    {
        normal,
        normalHover,
        clicked,
        clickedHover,
        disabled
    }
}