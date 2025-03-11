using System;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Serializable]
    public class BaseWaypointInfo
    {
        public string Description;
        public List<Transform> pointList = new();
    }

    [Serializable]
    public class GuestWaypointInfo : BaseWaypointInfo
    {
        public Utils.WaypointType type;
    }

    [Serializable]
    public class CarWaypointInfo : BaseWaypointInfo
    {
        public Utils.CarWaypointType type;
    }

    [Header("Guest Waypoint Settings")]
    [SerializeField] private List<GuestWaypointInfo> waypointInfoList = new();

    [Header("Car Waypoint Settings")]
    [SerializeField] private List<CarWaypointInfo> carWaypoints = new();

    public List<GuestWaypointInfo> GuestWaypoints => waypointInfoList;

    public List<CarWaypointInfo> CarWaypoints => carWaypoints;
}
