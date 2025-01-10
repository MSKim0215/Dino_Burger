using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TableController : MonoBehaviour
    {
        [SerializeField] private Hand hand;

        public void Take(GameObject takeObject) => hand.GetHandUp(takeObject);

        public GameObject Give()
        {
            GameObject tableObject = hand.HandUpObject;

            if(hand.HandUpObject != null)
            {
                hand.ClearHand();
            }

            return tableObject;
        }
    }
}