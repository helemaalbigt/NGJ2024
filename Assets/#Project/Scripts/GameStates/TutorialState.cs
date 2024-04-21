using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using UnityEngine;

public class TutorialState : MonoState
{
    public PageGroup pageGroup;
    public Page page;
    public VrButton button;
    public MonoState next;
    
    private void OnEnable() {
        pageGroup.OpenPage(page);
        SceneFinder.I.minesAvailableUI.SetActive(false);
        SceneFinder.I.detector.SetActive(false);
    }

    void Update() {
        if (button.interactableState == InteractableState.clicked) {
            GoToState(next);
        }
        
#if UNITY_EDITOR
        if (InputManager.I.PrimaryButtonDown(Hand.right)) {
            GoToState(next);
        }
#endif
    }
}
