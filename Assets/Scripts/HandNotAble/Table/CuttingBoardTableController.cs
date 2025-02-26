using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableControllerUseUI, IToolInterAction
    {
        [Header("Table View")]
        [SerializeField] private UI.TableView view;

        [Header("Tool Hand")]
        [SerializeField] private Hand toolHand = null;

        public bool IsCutOver => hand.GetHandUpComponent<HandAble.IngredientController>().IngredientState == Utils.IngredientState.CutOver;

        protected override void Initialize()
        {
            view.Initialize(this);
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null || hand.HandUpObject == null) return;

            bool isAlreadyStart = ingredient.CurrentCookTime > 0f && ingredient.CurrentCookTime < ingredient.MaximumCookTime;
            if (!isAlreadyStart) return;

            OnTriggerOriginActiveEvent(isAlreadyStart);
            OnTriggerValueEvent(ingredient.CurrentCookTime / ingredient.MaximumCookTime);
        }

        public void TakeTool(GameObject takeObject)
        {
            toolHand.GetHandUpHoldRotate(takeObject);
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

        public GameObject GiveTool()
        {
            var tool = toolHand.HandUpObject;
            if(tool != null)
            {
                toolHand.ClearHand();
            }

            return tool;
        }

        public void Cutting()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null || hand.HandUpObject == null) return;

            bool isTimeOver = ingredient.CurrentCookTime >= ingredient.MaximumCookTime;
            OnTriggerOriginActiveEvent(!isTimeOver);

            if (isTimeOver)
            {
                ingredient.SetIngredientState(Utils.IngredientState.CutOver);
                return;
            }

            ingredient.CurrentCookTime += Time.deltaTime * (1 + Managers.UserData.GetUpgradeAmount(Utils.ShopItemIndex.SHOP_PLAYER_CUTTING_SPEED_INDEX));
            ingredient.SetIngredientState(Utils.IngredientState.Cutting);
            OnTriggerValueEvent(ingredient.CurrentCookTime / ingredient.MaximumCookTime);
        }
    }
}