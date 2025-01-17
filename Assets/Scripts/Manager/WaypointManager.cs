using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class WaypointManager : MonoBehaviour 
    {
        [Serializable]
        private class WayPointInfo
        {
            public Utils.WaypointType type;
            public List<Transform> pointList = new();
        }

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
        [SerializeField] private List<WayPointInfo> waypointInfoList = new();

        private Dictionary<Utils.WaypointType, WayPointInfo> waypointDict = new();


        
        [SerializeField] private List<Transform> wayPointList = new();
        [SerializeField] private List<Transform> outsideRightPointList = new();      // 안들어가고 바로 나가는 경우
        [SerializeField] private List<Transform> outsideLeftPointList = new();      // 안들어가고 바로 나가는 경우

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

        public Vector3 GetOutsideRightPoint(int currIndex)
        {
            return outsideRightPointList[currIndex].position;
        }

        public Vector3 GetOutsideLeftPoint(int currIndex)
        {
            return outsideLeftPointList[currIndex].position;
        }
    }
}