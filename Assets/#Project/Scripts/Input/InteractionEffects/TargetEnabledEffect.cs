using Rowhouse;
using UnityEngine;

public class TargetEnabledEffect : Effect
{
    public GameObject target;
    public bool normal;
    public bool hover;
    public bool clicked;
    public bool clickedHover;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.normal:
                SetActive(normal);
                break;

            case InteractableState.normalHover:
                SetActive(hover);
                break;

            case InteractableState.clicked:
                SetActive(clicked);
                break;

            case InteractableState.clickedHover:
                SetActive(clickedHover);
                break;
        }
    }

    private void SetActive(bool active) {
        if (target != null) {
            target.SetActive(active);
        }
    }
}