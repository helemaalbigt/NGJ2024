using UnityEngine;
using Input = UnityEngine.Input;

namespace Rowhouse
{
    public class MockInput : InputSource
    {
        public override bool Trigger(Hand hand) {
            return Input.GetMouseButton(0);
        }

        public override float TriggerAxis(Hand hand) {
            return 0;
        }

        public override bool TriggerDown(Hand hand) {
            return Input.GetMouseButtonDown(0);
        }

        public override bool TriggerUp(Hand hand) {
            return Input.GetMouseButtonUp(0);
        }
        
        public override bool PrimaryButtonDown(Hand hand) {
            return Input.GetMouseButtonDown(1);
        }
    }
}