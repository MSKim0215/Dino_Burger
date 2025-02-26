using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TrashcanTableController : TableController
    {
        public override void Take(GameObject takeObject)
        {
            if(takeObject.TryGetComponent<HandAble.FoodController>(out var food))
            {
                food.Release();
                return;
            }

            if(takeObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                ingredient.Release();
                return;
            }
        }
    }
}