using MSKim.Manager;

namespace MSKim.Scene
{
    public class MultiGameScene : BaseScene
    {
        protected override void Initialize()
        {
            Managers.Instance.Initialize(Utils.SceneType.MultiGame);
        }
    }
}