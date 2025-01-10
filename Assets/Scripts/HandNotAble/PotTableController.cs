using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PotTableController : TableController
    {
        private void Update()
        {
            Boil();
        }

        private void Boil()
        {
            if (hand.HandUpObject == null) return;

            Debug.Log("끓이는 중~~");
        }
    }
}