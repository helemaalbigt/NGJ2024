using System.Collections;
using System.Collections.Generic;
using Meta.XR.Depth;
using UnityEngine;

public class DepthHandsRemover : MonoBehaviour {
    public EnvironmentDepthTextureProvider _depthTextureProvider;
    
    void Awake() {
        _depthTextureProvider.RemoveHands(true);
    }
}