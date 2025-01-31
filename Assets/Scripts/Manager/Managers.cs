using UnityEngine;

namespace MSKim.Manager
{
    public class Managers : MonoBehaviour
    {
        private static Managers instance;

        [Header("Manager List")]
        [SerializeField] private UserDataManager userDataManager = new();
        [SerializeField] private GameDataManager gameDataManager = new();
        [SerializeField] private GameManager gameManager = new();

        private Utils.SceneType currentSceneType = Utils.SceneType.Title;

        public static Managers Instance
        {
            get
            {
                Init();
                return instance;
            }
        }

        public static UserDataManager UserData => Instance.userDataManager;

        public static GameDataManager GameData => Instance.gameDataManager;

        public static GameManager Game => Instance.gameManager;

        public void Initialize(Utils.SceneType nextSceneType)
        {
            if (currentSceneType == nextSceneType) return;

            currentSceneType = nextSceneType;

            if (currentSceneType == Utils.SceneType.Title)
            {
                userDataManager.Initialize();
                gameDataManager.Initialize();
            }
            else if(currentSceneType == Utils.SceneType.MainGame)
            {
                gameManager.Initialize();
            }
        }

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

        private void Update()
        {
            if (currentSceneType == Utils.SceneType.Title)
            {
                userDataManager.OnUpdate();
            }
        }
    }
}