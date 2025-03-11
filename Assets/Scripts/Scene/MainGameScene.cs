using MSKim.Manager;

namespace MSKim.Scene
{
    public class MainGameScene : BaseScene
    {
        protected override void Initialize()
        {
            Managers.Instance.Initialize(Utils.SceneType.MainGame);
        }
    }
}