using System;
using UnityEngine;

namespace Rowhouse
{
    //Simple statemachine based on monobehaviours getting turned on and off.
    //Monobehaviours should have a childclass of MonoState, which can then handle the state using OnEnable, OnDisable and Update events.
    public class MonoStateMachine : MonoBehaviour
    {
        public event Action OnStateMachineExit;
        public event Action OnStateChanged;

        public MonoState startState;
        public bool startOnEnable;
        public bool logStateChanges;

        private MonoState _currentState;

        public void OnEnable() {
            if (startOnEnable) {
                GoToStart();
            }
        }

        public void GoToStart() {
            if (startState == null) {
                Debug.LogError("[MonoStateMachine] No StartState set.");
                return;
            }

            GoToNewState(startState);
        }

        public string GetCurrentState() {
            if (_currentState == null) {
#if !UNITY_EDITOR
                //Debug.LogError("[MonoStateMachine] Tried retrieving state type but no state is currently active");
#endif
                return "";
            }

            return _currentState.GetStateType();
        }

        private void GoToNewState(MonoState newState) {
            if (_currentState != null) {
                _currentState.gameObject.SetActive(false);
                _currentState.OnGoToState -= GoToNewState;
                _currentState.OnExitStateMachine -= ExitStateMachine;
            }
            _currentState = newState;
            
            if (logStateChanges) {
                Debug.Log($"[MonoStateMachine] {gameObject.name} State changed to {newState.gameObject.name} at time {Time.unscaledTime}");
            }
            
            OnStateChanged?.Invoke();

            newState.OnGoToState += GoToNewState;
            newState.OnExitStateMachine += ExitStateMachine;
            if (newState == null) {
                ExitStateMachine();
            } else {
                newState.gameObject.SetActive(true);
            }
        }

        private void ExitStateMachine() {
            if (_currentState != null) {
                _currentState.OnGoToState -= GoToNewState;
                _currentState.OnExitStateMachine -= ExitStateMachine;
            }

            foreach (var state in transform.GetComponentsInChildren<MonoState>()) {
                state.gameObject.SetActive(false);
            }

            OnStateMachineExit?.Invoke();
        }
    }
}