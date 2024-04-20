using Rowhouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventEffect : Effect
{
    public UnityEvent normal;
    public UnityEvent hover;
    public UnityEvent onClick;
    public UnityEvent onClickHover;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.normal:
                normal.Invoke();
                break;

            case InteractableState.normalHover:
                hover.Invoke();
                break;

            case InteractableState.clicked:
                onClick.Invoke();
                break;

            case InteractableState.clickedHover:
                onClickHover.Invoke();
                break;
        }
    }
}
