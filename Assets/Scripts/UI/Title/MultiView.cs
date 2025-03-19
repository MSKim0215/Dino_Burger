using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class MultiView
    {
        [Serializable]
        private class CommonButton
        {
            [SerializeField] protected Button button = null;

            public void OnClickEvent(UnityAction action)
            {
                button.onClick.AddListener(() =>
                {
                    action?.Invoke();
                });
            }
        }

        private MultiPopup controller;

        [SerializeField] private CommonButton backButton;
        [SerializeField] private GameObject loading;

        public void Initialize(MultiPopup controller)
        {
            this.controller = controller;
            BindEvent();
        }

        private void BindEvent()
        {
            backButton.OnClickEvent(controller.OnBackEvent);
        }

        public void ActiveLoading()
        {
            loading.SetActive(true);
        }

        public void UnActiveLoading() => loading.SetActive(false);
    }
}