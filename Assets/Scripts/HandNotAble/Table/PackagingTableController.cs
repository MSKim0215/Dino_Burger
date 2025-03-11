using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PackagingTableController : TableController
    {
        public override void Take(GameObject takeObject)
        {
            if(takeObject.TryGetComponent<HandAble.FoodController>(out var food))
            {
                food.CurrentFoodState = Utils.FoodState.Packaging;
                food.Packaing();
            }

            base.Take(takeObject);
        }
    }
}