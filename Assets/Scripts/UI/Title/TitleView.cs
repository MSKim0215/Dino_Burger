using MSKim.Manager;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class TitleView
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

        private Title controller;

        [SerializeField] private Transform uiRoot = null;
        [SerializeField] private CommonButton startButton = null;
        [SerializeField] private CommonButton multiButton = null;
        [SerializeField] private CommonButton matchButton = null;
        [SerializeField] private CommonButton shopButton = null;
        [SerializeField] private CommonButton exitButton = null;

        public Transform UIRoot => uiRoot;

        public void Initialize(Title controller)
        {
            this.controller = controller;
            BindEvent();
        }

        private void BindEvent()
        {
            startButton.OnClickEvent(controller.OnStartEvent);
            shopButton.OnClickEvent(controller.OnShopEvent);
            exitButton.OnClickEvent(controller.OnExitEvent);
            matchButton.OnClickEvent(Managers.Net.StartMatching);
        }
    }
}