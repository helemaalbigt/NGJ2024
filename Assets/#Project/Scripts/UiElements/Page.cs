using UnityEngine;

namespace Rowhouse
{
    //Abstract page in the UI. Opened/closed by a PageGroup
    public class Page : MonoBehaviour
    {
        public GameObject content;
        public Page back;

        public virtual void Open() {
            SetContentIfNull();
            content.SetActive(true);
        }

        public virtual void Close() {
            SetContentIfNull();
            content.SetActive(false);
        }

        protected void SetContentIfNull() {
            if (content == null)
                content = gameObject;
        }

        public void SetParent(Transform parent) {
            content.transform.SetParent(parent);
            content.transform.localPosition = Vector3.zero;
            content.transform.localRotation = Quaternion.identity;
        }

        public bool HasBack() {
            return back != null;
        }

        public bool IsOpen() {
            return content.activeInHierarchy;
        }
    }
}