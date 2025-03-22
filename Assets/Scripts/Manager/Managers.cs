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
        [SerializeField] private TitleManager titleManager = new();
        [SerializeField] private ObjectPoolManager objectPoolManager = new();
        [SerializeField] private WaypointManager titleWaypointManager = new();
        [SerializeField] private WaypointManager gameWaypointManager = new();
        [SerializeField] private NetManager netManager = new();

        private FileManager fileManager = new();

        private Utils.SceneType currentSceneType = Utils.SceneType.None;

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

        public static TitleManager Title => Instance.titleManager;

        public static ObjectPoolManager Pool => Instance.objectPoolManager;

        public static WaypointManager TitleWaypoint => Instance.titleWaypointManager;

        public static WaypointManager GameWaypoint => Instance.gameWaypointManager;

        public static FileManager File => Instance.fileManager;

        public static NetManager Net => Instance.netManager;

        public static Utils.SceneType CurrentSceneType => Instance.currentSceneType;

        public void Initialize(Utils.SceneType nextSceneType)
        {
            if (currentSceneType == nextSceneType) return;

            currentSceneType = nextSceneType;

            objectPoolManager.Initialize();

            if (currentSceneType == Utils.SceneType.Title)
            {
                netManager.Initialize();
                gameDataManager.Initialize();
                fileManager.Initialize();
                userDataManager.Initialize();
                titleManager.Initialize();
                titleWaypointManager.Initialize();
            }
            else if(currentSceneType == Utils.SceneType.MainGame)
            {
                gameManager.Initialize();
                gameWaypointManager.Initialize();
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
                titleManager.OnUpdate();
            }
            else if(currentSceneType == Utils.SceneType.MainGame)
            {
                gameManager.OnUpdate();
            }
        }
    }
}