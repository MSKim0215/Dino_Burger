using MSKim.Manager;
using System;
using TMPro;
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

            public void OnClickEvent(UnityAction<string> action, TMP_InputField filed)
            {
                button.onClick.AddListener(() =>
                {
                    action?.Invoke(filed.text);
                });
            }
        }

        private Title controller;

        [SerializeField] private Transform uiRoot = null;
        [SerializeField] private CommonButton startButton = null;
        [SerializeField] private CommonButton multiButton = null;
        [SerializeField] private GameObject multiSubPopup = null;
        [SerializeField] private CommonButton multiExitButton = null;
        [SerializeField] private CommonButton matchButton = null;
        [SerializeField] private CommonButton connectButton = null;
        [SerializeField] private TMP_InputField codeInput = null;
        [SerializeField] private TextMeshProUGUI createCodeText = null;
        [SerializeField] private CommonButton createButton = null;
        [SerializeField] private CommonButton shopButton = null;
        [SerializeField] private CommonButton exitButton = null;

        public Transform UIRoot => uiRoot;

        public void Initialize(Title controller)
        {
            this.controller = controller;
            BindEvent();

            Managers.Net.OnCreateLobbyEvent -= SetLobbyCode;
            Managers.Net.OnCreateLobbyEvent += SetLobbyCode;
        }

        private void BindEvent()
        {
            startButton.OnClickEvent(controller.OnStartEvent);
            multiButton.OnClickEvent(OnMultiOpen);
            shopButton.OnClickEvent(controller.OnShopEvent);
            exitButton.OnClickEvent(controller.OnExitEvent);
            matchButton.OnClickEvent(Managers.Net.StartMatching);
            multiExitButton.OnClickEvent(OnMultiClose);
            createButton.OnClickEvent(Managers.Net.CreateLobby);
            connectButton.OnClickEvent(Managers.Net.JoinGameWithCode, codeInput);
        }

        private void SetLobbyCode(string code) => createCodeText.text = code;

        private void OnMultiClose()
        {
            multiSubPopup.SetActive(false);
        }

        private void OnMultiOpen()
        {
            multiSubPopup.SetActive(true);
        }
    }
}