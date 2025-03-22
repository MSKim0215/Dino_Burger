using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;

namespace MSKim.Manager
{
    public class NetManager : BaseManager
    {
        private Lobby currentLobby;

        public override async void Initialize()
        {
            base.Initialize();

            await UnityServices.InitializeAsync();

            if(!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        public async void StartMatching()
        {
            if(!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("you are not signedIn.");
                return;
            }

            currentLobby = await FindAvailableLobby();

            if(currentLobby == null)
            {
                await CreateNewLobby();
            }
            else
            {
                await JoinLobby(currentLobby.Id);
            }
        }

        private async Task<Lobby> FindAvailableLobby()
        {
            try
            {
                var queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
                if(queryResponse.Results.Count > 0)
                {
                    return queryResponse.Results[0];
                }
            }
            catch(LobbyServiceException e)
            {
                Debug.Log($"Lobby Found Failed... => {e}");
            }
            return null;
        }

        private async Task CreateNewLobby()
        {
            try
            {
                currentLobby = await LobbyService.Instance.CreateLobbyAsync("Random Match Room", 2);
                
                Debug.Log($"Create New Room => {currentLobby.Id}");     
            
                await AllocateRelayServerAndJoin(currentLobby);

                StartHost();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"Lobby Create Failed... => {e}");
            }
        }

        private async Task JoinLobby(string lobbyId)
        {
            try
            {
                currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                
                Debug.Log($"Connect to Room! => {currentLobby.Id}");
                
                StartClient();
            }
            catch(LobbyServiceException e)
            {
                Debug.Log($"Lobby Connected Failed... => {e}");
            }
        }

        private async Task AllocateRelayServerAndJoin(Lobby lobby)
        {
            try
            {
                var allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
                var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                Debug.Log($"Releay Server Allocate Success...! JoinCode => {joinCode}");
            }
            catch(RelayServiceException e)
            {
                Debug.Log($"Releay Server Allocate Failed... => {e}");
            }
        }

        private void StartHost()
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Host Start");
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client Connected");
        }
    }
}