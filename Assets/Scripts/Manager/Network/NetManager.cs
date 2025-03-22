using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;

namespace MSKim.Manager
{
    public partial class NetManager : BaseManager
    {
        private Lobby currentLobby;

        private const int maxPlayers = 2;
        private string gameSceneName = "MainGame";

        public event Action<string> OnCreateLobbyEvent;

        public override async void Initialize()
        {
            base.Initialize();

            await UnityServices.InitializeAsync();

            if(!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}