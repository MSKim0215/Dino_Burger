using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableController
    {
        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.CuttingBoard);
            name = data.Name;
        }

        public override GameObject Give()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null) return base.Give();
            if (ingredient.IngredientState != Utils.IngredientState.CutOver) return base.Give();
            if (ingredient.YieldAmount <= 1) return base.Give();

            ingredient.YieldAmount--;

            return Instantiate(hand.HandUpObject);
        }

        public void Cutting()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.IngredientController>();
            if (ingredient == null || hand.HandUpObject == null) return;

            if (ingredient.CurrentCookTime >= ingredient.MaximumCookTime)
            {
                Debug.Log($"{ingredient.IngredientType}의 손질이 완료되었습니다.");
                ingredient.SetIngredientState(Utils.IngredientState.CutOver);
                return;
            }

            ingredient.CurrentCookTime += Time.deltaTime;
            ingredient.SetIngredientState(Utils.IngredientState.Cutting);
        }
    }
}