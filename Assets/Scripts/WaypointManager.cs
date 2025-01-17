using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class WaypointManager : MonoBehaviour 
    {
        private static WaypointManager instance;

        public static WaypointManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        [Header("Guest Waypoint Settings")]
        [SerializeField] private List<Transform> wayPointList = new();

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Vector3 GetCurrentWayPoint(int currentIndex)
        {
            return wayPointList[currentIndex].position;
        }
    }
}