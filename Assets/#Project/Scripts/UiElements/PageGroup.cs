using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rowhouse
{
    //Collection of pages, saved and referenced by their gameObject name
    //Manages which one is visible or not and makes sure only one is visible at a time
    //Use Back() to go up in the page hierarchy
    public class PageGroup : MonoBehaviour
    {
        public event Action<Page> OnPageChanged;

        public bool _collectPagesOnStart;
        public bool _unparentInactivePages;
        public Transform _pageParent;
        public List<Page> _pages = new();

        [Header("Debug")]
        [SerializeField]
        private Page _activePage;
        private Transform _inactiveParent;
        [FormerlySerializedAs("_logPageOpens")] public bool _logAllPageOpens;

        public void Awake() {
            if (_unparentInactivePages) {
                var go = new GameObject("PageGroup_" + gameObject.name + "_InactivePages");
                var canvas = go.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                go.transform.position = new Vector3(100f, -100f, 0);
                _inactiveParent = go.transform;
            }

            if (_collectPagesOnStart) {
                _pages.AddRange(GetComponentsInChildren<Page>(true));
            }

            foreach (var page in _pages) {
                if (_unparentInactivePages) {
                    page.Close();
                    page.SetParent(_inactiveParent);
                }
            }
        }

        void Update() {
            if (_activePage != null && GetActivePage() != _activePage) {
                
            }
        }

        public void AddPage(Page page) {
            if (!_pages.Contains(page)) {
                _pages.Add(page);

                if (_unparentInactivePages) {
                    page.SetParent(_inactiveParent);
                }
            }
        }

        public void RemovePage(Page page) {
            if (!_pages.Contains(page)) {
                _pages.Remove(page);
            }
        }

        public void OpenPage(string pageName) {
            foreach (var page in _pages) {
                if (page.gameObject.name.ToLower() == pageName.ToLower()) {
                    OpenPage(page);
                    return;
                }
            }
            
            Debug.LogError($"[PageGroup] Could not find page {pageName} in {gameObject.name}");
        }

        public void OpenPage(Page page) {
            if (GetActivePage() == page) {
                return;
            }

            if (_pages.Contains(page)) {
                CloseAll();
                page.Open();
                if (_unparentInactivePages) {
                    page.SetParent(_pageParent);
                }

                _activePage = page;

                if(_logAllPageOpens)
                    Debug.Log("[PageGroup] Opened page " + page.gameObject.name + " , activepage: " + _activePage.name + ", actual activePage: " + GetActivePage().name);
                
                OnPageChanged?.Invoke(page);
            } else {
                Debug.LogError("[PageGroup] Tried opening page " + page.gameObject.name + ", but could not find it");
            }
        }

        public void CloseAll() {
            foreach (var page in _pages) {
                page.Close();
                if (_unparentInactivePages && page.transform.parent != _inactiveParent) {
                    page.SetParent(_inactiveParent);
                }

                _activePage = null;
            }
        }

        public Page GetActivePage() {
            foreach (var page in _pages) {
                if (page.gameObject.activeSelf) {
                    return page;
                }
            }

            return null;
        }

        public void Back() {
            if (_activePage != null && _activePage.HasBack()) {
                OpenPage(_activePage.back);
            }
        }
    }
}