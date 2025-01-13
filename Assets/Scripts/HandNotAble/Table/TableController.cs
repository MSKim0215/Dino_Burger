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

        public GameObject HandUpObject => hand.HandUpObject;

        public bool IsHandEmpty => hand.HandUpObject == null;

        public Utils.CrateType HandUpObjectType => hand.HandUpObjectType;

        public bool IsHandUpObjectBurger() => hand.GetHandUpComponent<HandAble.BurgerFoodController>() != null;

        public virtual void Take(GameObject takeObject)
        {
            hand.GetHandUp(takeObject);
        }

        public virtual GameObject Give()
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