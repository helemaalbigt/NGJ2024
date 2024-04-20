using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rowhouse
{
//Takes input from PhysicsButton and passes it on to a buttons InteractionListener as input
    public class PhysicsButtonInteractor : MonoBehaviour
    {
        [FormerlySerializedAs("physicsButtonOld")] public PhysicsButton physicsButton;
        public InteractionListener listener;

        private void Awake() {
            physicsButton.OnClickDown += PhysicsButtonOnOnClickDown;
            physicsButton.OnClickUp += PhysicsButtonOnOnClickUp;
        }

        private void PhysicsButtonOnOnClickDown() {
            listener.ClickDown(GetData());
        }

        private void PhysicsButtonOnOnClickUp() {
            listener.ClickUp(GetData());
        }

        private InteractionData GetData() {
            return new InteractionData() {
                interactorType = typeof(PhysicsButton)
            };
        }

        private void OnDestroy() {
            physicsButton.OnClickDown -= PhysicsButtonOnOnClickDown;
            physicsButton.OnClickUp -= PhysicsButtonOnOnClickUp;
        }
    }
}
