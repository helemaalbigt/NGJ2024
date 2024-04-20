using System;
using Rowhouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrToggleGroup : MonoBehaviour
{
    public event Action<int> OnActiveToggleSwitched;
    
    public bool allowSwitchOff;

    private List<VrToggle> _toggles = new List<VrToggle>();
    private VrToggle _activeToggle;

    public void AddToggle(VrToggle toggle) {
        if (!_toggles.Contains(toggle))
            _toggles.Add(toggle);
    }

    public void Refresh() {
        foreach (var t in _toggles) {
            if (allowSwitchOff || t != _activeToggle) {
                t.Set(false, true);
            }
        }
    }

    public void SetActiveToggle(VrToggle toggle) {
        _activeToggle = toggle;
        if (!_activeToggle.isOn)
            _activeToggle.Set(true);
        
        OnActiveToggleSwitched?.Invoke(GetActiveToggleIndex());
        
        foreach (var t in _toggles) {
            if (t != _activeToggle) {
                t.Set(false);
            }
        }
    }

    public void SetActiveToggle(int index) {
        if (index < _toggles.Count) {
            SetActiveToggle(_toggles[index]);
            
            OnActiveToggleSwitched?.Invoke(index);
        } else {
            Debug.Log("[VrToggleGroup] tried to set a toggle by index " + index + " on group " + gameObject.name + ", but there was no toggle with this index");
        }
    }

    public void SetActiveToggle(string toggleName) {
        foreach (var toggle in _toggles) {
            if (toggle.gameObject.name == toggleName) {
                SetActiveToggle(toggle);
                return;
            }
        }

        Debug.Log("[VrToggleGroup] could not set toggle by name " + toggleName);
    }

    public bool HasSelection() {
        foreach (var t in _toggles) {
            if (t.isOn)
                return true;
        }
        return false;
    }

    public int GetActiveToggleIndex() {
        var index = 0;
        foreach (var t in _toggles) {
            if (t == _activeToggle) {
                return index;
            }
            index++;
        }
        return index;
    }

    public VrToggle GetSelected() {
        foreach (var t in _toggles) {
            if (t.isOn)
                return t;
        }
        return null;
    }
}
