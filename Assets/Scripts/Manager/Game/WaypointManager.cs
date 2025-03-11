using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class WaypointManager : BaseManager 
    {
        private readonly Dictionary<Utils.WaypointType, List<Transform>> waypointDict = new();
        private readonly Dictionary<Utils.CarWaypointType, List<Transform>> carWaypointDict = new();
        private Waypoints waypoints;

        public override void Initialize()
        {
            base.Initialize();

            if (waypoints != null) return;

            if (waypointDict.Count > 0)
            {
                waypointDict.Clear();
            }

            if (carWaypointDict.Count > 0)
            {
                carWaypointDict.Clear();
            }

            if(GameObject.Find("Waypoints").TryGetComponent(out waypoints))
            {
                foreach(var waypoint in waypoints.GuestWaypoints)
                {
                    waypointDict.Add(waypoint.type, waypoint.pointList);
                }

                foreach(var carWaypoint in waypoints.CarWaypoints)
                {
                    carWaypointDict.Add(carWaypoint.type, carWaypoint.pointList);
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

        public Vector3 GetCurrentWaypoint(Utils.CarWaypointType targetType, int currentIndex)
        {
            return carWaypointDict[targetType][currentIndex].position;
        }

        public int GetCurrentWaypointMaxIndex(Utils.CarWaypointType targetType)
        {
            return carWaypointDict[targetType].Count;
        }
    }
}