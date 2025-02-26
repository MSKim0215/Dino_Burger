using MSKim.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableControllerUseUI, IToolInterAction
    {
        [Header("Table View")]
        [SerializeField] private UI.TableView view;

        [Header("Tool Hand")]
        [SerializeField] private Hand toolHand = null;

        private Dictionary<Utils.CrateType, string> cratePrefabNameDict = new();

        public bool IsCutOver => hand.GetHandUpComponent<HandAble.IngredientController>().IngredientState == Utils.IngredientState.CutOver;

        protected override void Initialize()
        {
            for (int i = 0; i < Enum.GetValues(typeof(Utils.CrateType)).Length - 1; i++)
            {
                var type = (Utils.CrateType)i;
                cratePrefabNameDict.Add(type, $"Ingredient_{type}");
            }

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

            var createObj = Managers.Pool.GetPoolObject(cratePrefabNameDict[ingredient.IngredientType]);
            if (createObj.TryGetComponent<HandAble.IngredientController>(out var controller))
            {
                controller.Copy(ingredient);
            }

            ingredient.YieldAmount--;

            return createObj;
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