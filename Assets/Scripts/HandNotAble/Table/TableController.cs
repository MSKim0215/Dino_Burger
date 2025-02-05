using System;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TableControllerUseUI : TableController
    {
        private event Action<bool> OnActiveEvent;
        private event Action<float> OnValueEvent;

        public void OnSetUpActiveEvent(Action<bool> OnActiveEvent)
        {
            this.OnActiveEvent -= OnActiveEvent;
            this.OnActiveEvent += OnActiveEvent;
        }

        public void OnSetUpValueEvent(Action<float> OnValueEvent)
        {
            this.OnValueEvent -= OnValueEvent;
            this.OnValueEvent += OnValueEvent;
        }

        public void OnTriggerActiveEvent(bool isActive) => OnActiveEvent?.Invoke(isActive);

        public void OnTriggerValueEvent(float value) => OnValueEvent?.Invoke(value);
    }

    public class TableController : MonoBehaviour, IInterAction
    {
        [Header("Table Data Info")]
        [SerializeField] protected Data.TableData data;

        [Header("My Hand")]
        [SerializeField] protected Hand hand;

        public Utils.TableType TableType => data.Type;

        public GameObject HandUpObject => hand.HandUpObject;

        public bool IsHandEmpty => hand.HandUpObject == null;

        public Utils.CrateType HandUpObjectType => hand.HandUpObjectType;

        public bool IsHandUpObjectBurger() => hand.GetHandUpComponent<HandAble.BurgerFoodController>() != null;

        public bool IsHandUpObjectStew() => hand.GetHandUpComponent<HandAble.StewFoodController>() != null;

        private void Start()
        {
            Initialize();
        }

        protected virtual void Initialize() { }

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