using System;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Serializable]
    public class WayPointInfo
    {
        public string Description;
        public Utils.WaypointType type;
        public List<Transform> pointList = new();
    }

    [Header("Guest Waypoint Settings")]
    [SerializeField] private List<WayPointInfo> waypointInfoList = new();

    public List<WayPointInfo> WaypointInfoList => waypointInfoList;
}
