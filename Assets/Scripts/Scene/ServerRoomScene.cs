using MSKim.Manager;

namespace MSKim.Scene
{
    public class ServerRoomScene : BaseScene
    {
        protected override void Initialize()
        {
            Managers.Instance.Initialize(Utils.SceneType.ServerRoom);
        }
    }
}