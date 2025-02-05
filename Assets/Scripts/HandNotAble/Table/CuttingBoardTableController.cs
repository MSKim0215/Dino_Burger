using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableControllerUseUI
    {
        [Header("Table View")]
        [SerializeField] private UI.TableView view;

        [Header("Objects")]
        [SerializeField] private GameObject knifeObj = null;

        public void SetActiveKnife(bool isActive) => knifeObj.SetActive(isActive);

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

            OnTriggerOriginActiveEvent(isAlreadyStart);
            OnTriggerValueEvent(ingredient.CurrentCookTime / ingredient.MaximumCookTime);
        }

        public override GameObject Give()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null) return base.Give();
            if (ingredient.IngredientState != Utils.IngredientState.CutOver)
            {
                OnTriggerOriginActiveEvent(false);
                return base.Give();
            }
            if (ingredient.YieldAmount <= 1) return base.Give();

            ingredient.YieldAmount--;

            return Instantiate(hand.HandUpObject);
        }

        public bool IsCutOver => hand.GetHandUpComponent<HandAble.IngredientController>().IngredientState == Utils.IngredientState.CutOver;

        public void Cutting()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null || hand.HandUpObject == null) return;

            if (knifeObj.activeSelf) SetActiveKnife(false);

            bool isTimeOver = ingredient.CurrentCookTime >= ingredient.MaximumCookTime;
            OnTriggerOriginActiveEvent(!isTimeOver);

            if (isTimeOver)
            {
                ingredient.SetIngredientState(Utils.IngredientState.CutOver);
                SetActiveKnife(true);
                return;
            }

            ingredient.CurrentCookTime += Time.deltaTime;
            ingredient.SetIngredientState(Utils.IngredientState.Cutting);
            OnTriggerValueEvent(ingredient.CurrentCookTime / ingredient.MaximumCookTime);
        }
    }
}