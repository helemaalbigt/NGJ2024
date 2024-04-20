using System;
using UnityEngine;

namespace Rowhouse
{
    //Simple base class for a state in a MonoStateMachine. Should be disabled in the scene by default
    //Derive from this and use OnEnable, Update and OnDisable to manage state behaviour.
    //Call GoToState to go to another state
    //Call ExitStateMachine to finish this particualr statemachine
    public class MonoState : MonoBehaviour
    {
        public event Action<MonoState> OnGoToState;
        public event Action OnExitStateMachine;

        public string GetStateType() {
            return gameObject.name;// this.GetType().Name;
        }

        protected void GoToState(MonoState nextState) {
            if (nextState == null) {
                Debug.LogError("[MonoState] GoToState next state can't be null");
                return;
            }
            OnGoToState?.Invoke(nextState);
        }

        protected void ExitStateMachine() {
            OnExitStateMachine?.Invoke();
        }
    }
}

