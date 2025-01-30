using UnityEngine;

namespace MSKim.Manager
{
    public class Managers : MonoBehaviour
    {
        private static Managers instance;

        [Header("Manager List")]
        [SerializeField] private UserDataManager userData = new();

        private Utils.SceneType currentSceneType = Utils.SceneType.Title;

        public static Managers Instance
        {
            get
            {
                Init();
                return instance;
            }
        }

        public static UserDataManager UserData => Instance.userData;

        private void Start()
        {
            Init();
        }

        private static void Init()
        {
            if (instance == null)
            {
                var managers = GameObject.Find("@Managers");
                if(managers == null)
                {
                    managers = new() { name = "@Managers" };
                    managers.AddComponent<Managers>();
                }

                DontDestroyOnLoad(managers);
                managers.TryGetComponent(out instance);
            }
        }

        public void Initialize()
        {
            if(currentSceneType == Utils.SceneType.Title)
            {
                userData.Initialize();
            }
        }
    }
}