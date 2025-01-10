using UnityEngine;

namespace MSKim.HandNotAble
{
    public class GasStoveTableController:TableController
    {
        private void Update()
        {
            Grill();
        }

        private void Grill()
        {
            if (hand.HandUpObject == null) return;

            Debug.Log("굽는 중!~~!~!");
        }
    }
}