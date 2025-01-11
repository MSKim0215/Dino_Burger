using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CuttingBoardTableController : TableController
    {
        public void Cutting()
        {
            var ingredient = hand.GetHandUpIngredient() as HandAble.CheeseIngredientController;
            if (ingredient == null || hand.HandUpObject == null) return;

            if (ingredient.CurrentCookTime >= Utils.CUTTING_CHEESE_COOK_TIME)
            {
                Debug.Log($"{ingredient.IngredientType}의 손질이 완료되었습니다.");
                return;
            }

            ingredient.CurrentCookTime += Time.deltaTime;
        }
    }
}