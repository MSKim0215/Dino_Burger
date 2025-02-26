using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TrashcanTableController : TableController
    {
        public override void Take(GameObject takeObject)
        {
            Destroy(takeObject);
        }
    }
}