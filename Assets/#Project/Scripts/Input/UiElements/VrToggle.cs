using System;

namespace Rowhouse
{
    public class VrToggle : Interactable
    {
        public event Action OnHoverOn;
        public event Action OnHoverOff;
        public event Action<bool> OnToggle;
        public event Action<bool> OnToggleByuser;

        public bool isOn;
        public VrToggleGroup toggleGroup;
        public bool _allowHoverWhileInactive;
        public bool _interactionOnClickUp;

        private bool _hovered;
        private bool _clickDown = false;

        public void Start() {
            if (toggleGroup != null) {
                toggleGroup.AddToggle(this);
            }

            cursorInteractions.OnHoverOn += OnCursorEnter;
            cursorInteractions.OnHoverOff += OnCursorExit;
            if (_interactionOnClickUp) {
                cursorInteractions.OnClickUp += OnClick;
            } else {
                cursorInteractions.OnClickDown += OnClick;
            }
        }

        private void OnDestroy() {
            cursorInteractions.OnHoverOn -= OnCursorEnter;
            cursorInteractions.OnHoverOff -= OnCursorExit;
            if (_interactionOnClickUp) {
                cursorInteractions.OnClickUp -= OnClick;
            } else {
                cursorInteractions.OnClickDown -= OnClick;
            }
        }

        public void Update() {
            if (!isOn && (!_hovered || !active)) {
                interactableState = InteractableState.normal;
            } else if (!isOn && _hovered) {
                interactableState = InteractableState.normalHover;
            } else if (isOn && (!_hovered || !active)) {
                interactableState = InteractableState.clicked;
            } else {
                interactableState = InteractableState.clickedHover;
            }
        }

        public void Toggle() {
            if (toggleGroup != null) {
                if (toggleGroup.allowSwitchOff && isOn) {
                    isOn = false;
                } else {
                    toggleGroup.SetActiveToggle(this);
                    return; //event is fired from toggle group
                }
            } else {
                isOn = !isOn;
            }

            FireOnToggle();
        }

        public void Set(bool on) {
            Set(on, true);
        }

        public void Set(bool on, bool fireEvent) {
            isOn = on;
            if (fireEvent) {
                FireOnToggle();
            }
        }

        protected void OnCursorEnter(InteractionData data) {
            if (active || _allowHoverWhileInactive) {
                if (!_hovered) {
                    _hovered = true;
                    FireOnHoverOn();
                }
            }
        }

        protected void OnCursorExit(InteractionData data) {
            if (_hovered) {
                _hovered = false;
                FireOnHoverOff();
            }
        }

        private void OnClick(InteractionData data) {
            if (active) {
                Toggle();
                //Debug.Log("Toggle by click from " + cursor.hand + ", value " + isOn);
                FireOnToggleByUser();
            }
        }

        private void OnDisable() {
            _hovered = false;
        }

        private void FireOnHoverOn() {
            if (OnHoverOn != null) {
                OnHoverOn();
            }
        }

        private void FireOnHoverOff() {
            if (OnHoverOff != null) {
                OnHoverOff();
            }
        }

        private void FireOnToggle() {
            if (OnToggle != null) {
                OnToggle(isOn);
            }
        }

        private void FireOnToggleByUser() {
            if (OnToggleByuser != null) {
                OnToggleByuser(isOn);
            }
        }
    }
}