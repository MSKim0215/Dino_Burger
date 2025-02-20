using System;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TableControllerUseUI : TableController
    {
        private event Action<bool> OnOriginActiveEvent;
        private event Action<bool> OnChangeActiveEvent;
        private event Action<float> OnValueEvent;
        private event Action<Utils.CrateType> OnInputIngredientEvent;
        private event Action OnOutputIngredientEvent;
        private event Action OnValueCompleteEvent;

        public void OnSetupOriginActiveEvent(Action<bool> OnOriginActiveEvent)
        {
            this.OnOriginActiveEvent -= OnOriginActiveEvent;
            this.OnOriginActiveEvent += OnOriginActiveEvent;
        }

        public void OnSetupChangeActiveEvent(Action<bool> OnChangeActiveEvent)
        {
            this.OnChangeActiveEvent -= OnChangeActiveEvent;
            this.OnChangeActiveEvent += OnChangeActiveEvent;
        }

        public void OnSetupValueEvent(Action<float> OnValueEvent)
        {
            this.OnValueEvent -= OnValueEvent;
            this.OnValueEvent += OnValueEvent;
        }

        public void OnSetupInputIngredientEvent(Action<Utils.CrateType> OnTakeIngredientEvent)
        {
            this.OnInputIngredientEvent -= OnTakeIngredientEvent;
            this.OnInputIngredientEvent += OnTakeIngredientEvent;
        }

        public void OnSetupOutputIngredientEvent(Action OnOutputIngredientEvent)
        {
            this.OnOutputIngredientEvent -= OnOutputIngredientEvent;
            this.OnOutputIngredientEvent += OnOutputIngredientEvent;
        }

        public void OnSetupValueCompleteEvent(Action OnValueCompleteEvent)
        {
            this.OnValueCompleteEvent -= OnValueCompleteEvent;
            this.OnValueCompleteEvent += OnValueCompleteEvent;
        }

        public void OnTriggerOriginActiveEvent(bool isActive) => OnOriginActiveEvent?.Invoke(isActive);

        public void OnTriggerChangeActiveEvent(bool isActive) => OnChangeActiveEvent?.Invoke(isActive);

        public void OnTriggerValueEvent(float value) => OnValueEvent?.Invoke(value);

        public void OnTriggerInputIngredientEvent(Utils.CrateType type) => OnInputIngredientEvent?.Invoke(type);

        public void OnTriggerOutputIngredientEvent() => OnOutputIngredientEvent?.Invoke();

        public void OnTriggerValueCompleteEvent() => OnValueCompleteEvent?.Invoke();
    }

    public class TableController : InterActionMonoBehaviour
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

        public override void Take(GameObject takeObject)
        {
            hand.GetHandUp(takeObject);
        }

        public override GameObject Give()
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