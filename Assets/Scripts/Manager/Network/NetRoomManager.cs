using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using UnityEngine;

namespace MSKim.Manager
{
    public partial class NetManager : BaseManager
    {
        public async void JoinGameWithCode(string joinCode)
        {
            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.Log("JoinCode is Error.");
                return;
            }

            try
            {
                var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                    );

                StartClient();

                Debug.Log("Join Game with Code Success!");
            }
            catch (RelayServiceException e)
            {
                Debug.Log($"Game Join Failed... => {e}");
            }
        }

        public async void StartMatching()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("you are not signedIn.");
                return;
            }

            currentLobby = await FindAvailableLobby();

            if (currentLobby == null)
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
                if (queryResponse.Results.Count > 0)
                {
                    return queryResponse.Results[0];
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"Lobby Found Failed... => {e}");
            }
            return null;
        }

        public async void CreateLobby()
        {
            await CreateNewLobby();
        }

        private async Task CreateNewLobby()
        {
            try
            {
                currentLobby = await LobbyService.Instance.CreateLobbyAsync("Random Match Room", maxPlayers);

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
            catch (LobbyServiceException e)
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
                OnCreateLobbyEvent?.Invoke(joinCode);

                Debug.Log($"Releay Server Allocate Success...! JoinCode => {joinCode}");
            }
            catch (RelayServiceException e)
            {
                Debug.Log($"Releay Server Allocate Failed... => {e}");
            }
        }

        private void StartHost()
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Host Start");

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnHostDisconnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            OnPlayerJoined();
        }

        private void OnHostDisconnected(ulong clientId)
        {
            if(NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnHostDisconnected;
            }
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client Connected");
        }
    }
}