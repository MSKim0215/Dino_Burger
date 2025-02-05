using MSKim.Manager;
using System;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableController
    {
        [Header("Table View")]
        [SerializeField] private UI.CuttingBoardTableView view;

        public event Action<bool> OnActiveEvent;
        public event Action<float> OnValueEvent;

        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.CuttingBoard);
            name = data.Name;

            view.Initialize(this);
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null || hand.HandUpObject == null) return;

            bool isAlreadyStart = ingredient.CurrentCookTime > 0f;
            if (!isAlreadyStart) return;

            OnActiveEvent?.Invoke(isAlreadyStart);
            OnValueEvent?.Invoke(ingredient.CurrentCookTime / ingredient.MaximumCookTime);
        }

        public override GameObject Give()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null) return base.Give();
            if (ingredient.IngredientState != Utils.IngredientState.CutOver)
            {
                OnActiveEvent?.Invoke(false);
                return base.Give();
            }
            if (ingredient.YieldAmount <= 1) return base.Give();

            ingredient.YieldAmount--;

            return Instantiate(hand.HandUpObject);
        }

        public void Cutting()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null || hand.HandUpObject == null) return;

            bool isTimeOver = ingredient.CurrentCookTime >= ingredient.MaximumCookTime;
            OnActiveEvent?.Invoke(!isTimeOver);

            if (isTimeOver)
            {
                ingredient.SetIngredientState(Utils.IngredientState.CutOver);
                return;
            }

            ingredient.CurrentCookTime += Time.deltaTime;
            ingredient.SetIngredientState(Utils.IngredientState.Cutting);
            OnValueEvent?.Invoke(ingredient.CurrentCookTime / ingredient.MaximumCookTime);
        }
    }
}