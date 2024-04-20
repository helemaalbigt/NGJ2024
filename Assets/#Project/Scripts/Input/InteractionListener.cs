using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rowhouse
{
    public class InteractionListener : MonoBehaviour
    {
        public event Action<InteractionData> OnHoverOn;
        public event Action<InteractionData> OnHoverOff;
        public event Action<InteractionData> OnClickDown;
        public event Action<InteractionData> OnClickUp;

        public Transform interactionCenter;

        protected InteractionData _lastInteractionData;

        [Header("Debug")]
        [SerializeField]
        private List<GameObject> _hoveredInteractors = new();
        [SerializeField]
        private bool _hovered;
        [SerializeField]
        private bool _logInteractions;

        private void Awake() {
            if (interactionCenter == null) {
                interactionCenter = transform;
            }
        }

        private void OnDisable() {
            ForceRemoveInteractions();
        }

        private void OnDestroy() {
            ForceRemoveInteractions();
        }

        //TODO: Clean up. Replace with locally handdled management of this state
        public virtual bool IsHovered() {
            return _hovered;
        }

        public virtual InteractionData GetLastInteractionData() {
            return _lastInteractionData;
        }

        public void HoverOn(InteractionData data) {
            if (_logInteractions) {
                Debug.Log(gameObject.name + " Hovered on by " + data.hand);
            }

            _hovered = true;
            _lastInteractionData = data;

            if (data.interactorGameObject != null && !_hoveredInteractors.Contains(data.interactorGameObject)) {
                _hoveredInteractors.Add(data.interactorGameObject);
            }

            OnHoverOn?.Invoke(data);
        }

        public void HoverOff(InteractionData data) {
            if (_logInteractions) {
                Debug.Log(gameObject.name + " Hovered off by " + data.hand);
            }

            _hovered = false;
            _lastInteractionData = data;

            if (_hoveredInteractors.Contains(data.interactorGameObject)) {
                _hoveredInteractors.Remove(data.interactorGameObject);
            }

            if (_hoveredInteractors.Count == 0) {
                OnHoverOff?.Invoke(data);
            }
        }

        public void ClickDown(InteractionData data) {
            if (_logInteractions) {
                Debug.Log(gameObject.name + " clicked down by " + data.hand);
            }

            _lastInteractionData = data;

            OnClickDown?.Invoke(data);
        }

        public void ClickUp(InteractionData data) {
            if (_logInteractions) {
                Debug.Log(gameObject.name + " clicked up by " + data.hand);
            }

            _lastInteractionData = data;

            OnClickUp?.Invoke(data);
        }

        public void ForceRemoveInteractions() {
            if (_logInteractions) {
                Debug.Log("[InteractionListener] Force removed all interactions on" + gameObject.name);
            }

            _hovered = false;
            _hoveredInteractors.Clear();
        }
    }

    [Serializable]
    public class InteractionData
    {
        public Hand hand;
        public GameObject interactorGameObject;
        public Transform interactionTransform;
        public Type interactorType;
    }
}