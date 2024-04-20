using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps duplicate VrToggleGroups in sync
namespace Rowhouse
{
    public class VrToggleGroupSync : MonoBehaviour
    {
        public static event Action OnToggleGroupMapUpdated;

        public string tag;
        public VrToggleGroup toggleGroup;

        public static Dictionary<string, int> activeToggleMap;

        private void Awake() {
            InitIfNeeded();

            toggleGroup.OnActiveToggleSwitched += SyncToggle;
            OnToggleGroupMapUpdated += SetToggle;
        }

        private void OnDestroy() {
            toggleGroup.OnActiveToggleSwitched -= SyncToggle;
            OnToggleGroupMapUpdated -= SetToggle;

            activeToggleMap = null;
        }

        private void SyncToggle(int toggle) {
            InitIfNeeded();

            if (!toggleGroup.HasSelection())
                return;

            if (activeToggleMap.ContainsKey(tag)) {
                if (activeToggleMap[tag] != toggle) {
                    activeToggleMap[tag] = toggle;
                    OnToggleGroupMapUpdated?.Invoke();
                }
            } else {
                activeToggleMap.Add(tag, toggle);
                OnToggleGroupMapUpdated?.Invoke();
            }
        }

        private void SetToggle() {
            InitIfNeeded();

            if (activeToggleMap.ContainsKey(tag) && activeToggleMap[tag] != toggleGroup.GetActiveToggleIndex()) {
                toggleGroup.SetActiveToggle(activeToggleMap[tag]);
            }
        }

        private void InitIfNeeded() {
            if (activeToggleMap == null)
                activeToggleMap = new Dictionary<string, int>();
        }
    }
}
