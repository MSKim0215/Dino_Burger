using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class WaypointManager : BaseManager 
    {
        private readonly Dictionary<Utils.WaypointType, List<Transform>> waypointDict = new();
        private Waypoints waypoints;

        public override void Initialize()
        {
            base.Initialize();

            if (waypoints != null) return;

            if(GameObject.Find("Waypoints").TryGetComponent(out waypoints))
            {
                foreach(var waypoint in waypoints.WaypointInfoList)
                {
                    waypointDict.Add(waypoint.type, waypoint.pointList);
                }
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