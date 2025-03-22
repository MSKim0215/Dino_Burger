using Unity.Netcode;

namespace MSKim.Manager
{
    public partial class NetManager : BaseManager
    {
        private void OnPlayerJoined()
        {
            if(NetworkManager.Singleton.ConnectedClients.Count >= maxPlayers)
            {
                ChangeSceneForAllPlayers();
            }
        }

        private void ChangeSceneForAllPlayers()
        {
            if(NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
}