using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class WaypointManager : BaseManager 
    {
        [Serializable]
        private class WayPointInfo
        {
            public Utils.WaypointType type;
            public List<Transform> pointList = new();
        }

        [Header("Guest Waypoint Settings")]
        [SerializeField] private List<WayPointInfo> waypointInfoList = new();

        private readonly Dictionary<Utils.WaypointType, List<Transform>> waypointDict = new();

        public override void Initialize()
        {
            base.Initialize();

            foreach (var waypoint in waypointInfoList)
            {
                waypointDict.Add(waypoint.type, waypoint.pointList);
            }
        }

        public Vector3 GetCurrentWaypoint(Utils.WaypointType targetType, int currentIndex)
        {
            return waypointDict[targetType][currentIndex].position;
        }

        public int GetCurrentWaypointMaxIndex(Utils.WaypointType targetType)
        {
            return waypointDict[targetType].Count;
        }
    }
}