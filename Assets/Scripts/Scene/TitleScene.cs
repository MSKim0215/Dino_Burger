using MSKim.Manager;

namespace MSKim.Scene
{
    public class TitleScene : BaseScene
    {
        protected override void Initialize()
        {
            Managers.Instance.Initialize(Utils.SceneType.Title);
        }
    }
}