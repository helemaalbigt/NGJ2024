using UnityEngine;

namespace Rowhouse
{
    public class PhysicsButtonHeightEffect : Effect
    {
        public float disabled;
        public float normal;
        public float hover;
        public float clicked;
        public float clickedHover;

        [Space(25)]
        public PhysicsButton button;
        public bool updateWhenDisabled = true;

        private float _targetHeight;

        private void OnEnable() {
            StopAllCoroutines();
            SetHeight(_targetHeight);
        }

        private void OnDisable() {
            StopAllCoroutines();
            if (updateWhenDisabled) {
                SetHeight(_targetHeight);
            }
        }

        protected override void OnStateChange(InteractableState state) {
            switch (state) {
                case InteractableState.disabled:
                    ChangeHeight(disabled);
                    break;

                case InteractableState.normal:
                    ChangeHeight(normal);
                    break;

                case InteractableState.normalHover:
                    ChangeHeight(hover);
                    break;

                case InteractableState.clicked:
                    ChangeHeight(clicked);
                    break;

                case InteractableState.clickedHover:
                    ChangeHeight(clickedHover);
                    break;
            }
        }

        private void ChangeHeight(float height) {
            if (updateWhenDisabled || enabled) {
                _targetHeight = height;
                SetHeight(height);
            }
        }

        private void SetHeight(float height) {
            if (button != null) {
                button.upperLimit = height;
            }
        }
    }
}