using System;
using UnityEngine;

namespace Rowhouse
{
    public class VrButton : Interactable
    {
        public event Action OnClick;
        public event Action OnClickUp;

        public bool pressedThisFrame;

        [Header("Debug")]
        public bool __clickDown;
        public bool __clickUp;

        private void Start() {
            cursorInteractions.OnHoverOn += OnCursorEnter;
            cursorInteractions.OnHoverOff += OnCursorExit;
            cursorInteractions.OnClickDown += ClickDown;
            cursorInteractions.OnClickUp += ClickUp;
        }

        private void OnDestroy() {
            cursorInteractions.OnHoverOn -= OnCursorEnter;
            cursorInteractions.OnHoverOff -= OnCursorExit;
            cursorInteractions.OnClickDown -= ClickDown;
            cursorInteractions.OnClickUp -= ClickUp;
        }
        
        private void OnDisable() {
            pressedThisFrame = false;
            interactableState = active ? InteractableState.normal : InteractableState.disabled;
        }

        private void Update() {
#if UNITY_EDITOR
            if (__clickDown) {
                ClickDown(new InteractionData {
                    hand = Hand.right
                });
                __clickDown = false;
            }

            if (__clickUp) {
                ClickUp(new InteractionData {
                    hand = Hand.right
                });
                __clickUp = false;
            }
#endif
        }

        private void LateUpdate() {
            pressedThisFrame = false;
        }

        private void OnCursorEnter(InteractionData data) {
            if (active) {
                interactableState = InteractableState.normalHover;
            }
        }

        private void OnCursorExit(InteractionData data) {
            if (active) {
                interactableState = InteractableState.normal;
            }
        }

        private void ClickDown(InteractionData data) {
            if (active && interactableState != InteractableState.clicked) {
                interactableState = InteractableState.clicked;
                pressedThisFrame = true;

                OnClick?.Invoke();
            }
        }

        private void ClickUp(InteractionData data) {
            if (active) {
                if (cursorInteractions.IsHovered()) {
                    interactableState = InteractableState.normalHover;
                } else {
                    interactableState = InteractableState.normal;
                }

                OnClickUp?.Invoke();
            }
        }
    }
}