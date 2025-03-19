using UnityEngine;

namespace MSKim.UI
{
    public class MultiPopup : PoolAble
    {
        [Header("UI Settings")]
        [SerializeField] private MultiView view;

        private void Start()
        {
            view.Initialize(this);
        }

        public void OnBackEvent()
        {
            Release();
        }
    }
}