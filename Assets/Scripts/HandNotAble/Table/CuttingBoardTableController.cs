using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableController
    {
        public void Cutting()
        {
            var ingredient = hand.GetHandUpIngredient();
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