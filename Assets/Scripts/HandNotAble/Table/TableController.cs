using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TableController : MonoBehaviour, IInterAction
    {
        [Header("Table Type")]
        [SerializeField] private Utils.TableType tableType;

        [Header("My Hand")]
        [SerializeField] protected Hand hand;

        public Utils.TableType TableType => tableType;

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