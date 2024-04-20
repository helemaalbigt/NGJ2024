using Rowhouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapEffect : Effect
{
    public Material normal;
    public Material hover;
    public Material clicked;
    public Material clickedHover;

    [Space(25)]
    public Renderer _renderer;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.normal:
                SetMaterial(normal);
                break;
            case InteractableState.normalHover:
                SetMaterial(hover);
                break;
            case InteractableState.clicked:
                SetMaterial(clicked);
                break;
            case InteractableState.clickedHover:
                SetMaterial(clickedHover);
                break;
        }
    }

    public void SetMaterial(Material mat) {
        _renderer.material = mat;
    }
}
