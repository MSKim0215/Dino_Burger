using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class ZoneManager
    {
        private List<GameObject> pickupTableList = new();
        private List<GameObject> waitChairList = new();

        public void Initialize()
        {
            pickupTableList.Clear();
            waitChairList.Clear();

            var pickupSeats = GameObject.FindGameObjectsWithTag("PickupTable");
            pickupTableList.AddRange(pickupSeats);
  
            var waitSeats = GameObject.Find("Chairs").transform;
            for (int i = 0; i < waitSeats.childCount; i++)
            {
                waitChairList.Add(waitSeats.GetChild(i).gameObject);
            }
        }

        public int GetPickupTableCount() => pickupTableList.Count;

        public int GetWaitChairCount() => waitChairList.Count;
    }
}