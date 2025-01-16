using System.Collections.Generic;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : MonoBehaviour
    {
        [Header("Waypoint Settings")]
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private List<Transform> wayPointList = new();

        private void Update()
        {
            transform.position =
              Vector3.MoveTowards(gameObject.transform.position, wayPointList[currentPointIndex].transform.position, 0.01f);
            
            if(currentPointIndex < 2)
            {
                if (Vector3.Distance(wayPointList[currentPointIndex].position, transform.position) <= 0.01f)
                {
                    currentPointIndex++;
                }
            }
        }
    }
}