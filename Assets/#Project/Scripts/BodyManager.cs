using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrDebugPlugin;

public class BodyManager : MonoBehaviour
{
    public OVRSkeleton _skeleton;
    public Transform _head;
    public PlayerMineCollider _playerMineCollider;
    
    private Dictionary<OVRSkeleton.BoneId, Transform> _bones = new Dictionary<OVRSkeleton.BoneId, Transform>(); 

    void Update() {
        UpdateBones();
        VrDebug.DrawPoint(GetFootPosition());
        if(_playerMineCollider != null)
            _playerMineCollider.SetPosition(GetFootPosition());
    }

    public Vector3 GetFootPosition() {
        var pos = _head.position;
        pos.y = 0;
        return pos;
    }

    private void UpdateBones() {
        if (_skeleton.IsInitialized && _skeleton.IsDataValid) {
            var bones = _skeleton.Bones;

            for (var i = 0; i < bones.Count; i++) {
                var ovrBone = bones[i];
                if (!_bones.ContainsKey(ovrBone.Id)) {
                    _bones.Add(ovrBone.Id, ovrBone.Transform);
                } else {
                    _bones[ovrBone.Id] = ovrBone.Transform;
                    VrDebug.DrawAxis(ovrBone.Transform.position, ovrBone.Transform.rotation, 0.01f);
                }
            }
        }
    }
}
