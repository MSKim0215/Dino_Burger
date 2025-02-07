using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PackagingTableController : TableController
    {
        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.Packaging);
            name = data.Name;
        }

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