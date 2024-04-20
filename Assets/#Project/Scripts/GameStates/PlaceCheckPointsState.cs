using System;
using System.Collections;
using System.Collections.Generic;
using Rowhouse;
using Unity.VisualScripting;
using UnityEngine;
using Page = Rowhouse.Page;

public class PlaceCheckPointsState : MonoState {
    public CheckPointManager checkPointManager;
    public Transform rightHand;
    public PageGroup pageGroup;
    public Page page;
    public VrButton button;

    [Space(15)] public MonoState next;

    private CheckPoint _placingCheckpoint;
    
    private 
    
    void OnEnable() {
        checkPointManager.ClearAllCheckpoints();
        pageGroup.OpenPage(page);
    }

    private void Update() {
        HandlePlacement();

        if (checkPointManager.CheckPointCount > 0) {
            button.gameObject.SetActive(true);
            
            if (button.interactableState == InteractableState.clicked) {
                GoToState(next);
            }
            
#if UNITY_EDITOR
            if (InputManager.I.PrimaryButtonDown(Hand.right)) {
                GameManager.Instance.StartGame(1);
                GoToState(next);
            }
#endif
        } else {
            button.gameObject.SetActive(false);
        }
    }

    private void HandlePlacement() {
        if (InputManager.I.TriggerDown(Hand.right)) {
            _placingCheckpoint = checkPointManager.CreateCheckPoint();
            _placingCheckpoint.placing = true;
        }

        if (_placingCheckpoint != null) {
            _placingCheckpoint.transform.position = GetPlacementPos();
            _placingCheckpoint.isValidPos = checkPointManager.IsValidPlacementPos(GetPlacementPos());
            
            if (InputManager.I.TriggerUp(Hand.right)) {
                if (checkPointManager.TryAddCheckPoint(_placingCheckpoint)) {
                    _placingCheckpoint.placing = false;
                } else {
                    Destroy(_placingCheckpoint.gameObject);
                }

                _placingCheckpoint = null;
            }
        }
    }

    private Vector3 GetPlacementPos() {
        return new Vector3(rightHand.position.x, 0, rightHand.position.z);
    }
}
