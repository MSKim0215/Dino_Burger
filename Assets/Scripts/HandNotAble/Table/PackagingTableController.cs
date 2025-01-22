using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PackagingTableController : TableController
    {
        protected override void Initialize()
        {
            data = GameDataManager.Instance.GetTableData(Utils.TableType.Packaging);
            name = data.Name;
        }

        private bool isPackaging = false;

        public override void Take(GameObject takeObject)
        {
            if(takeObject.TryGetComponent<HandAble.FoodController>(out var food))
            {
                food.CurrentFoodState = Utils.FoodState.Packaging;
            }

            base.Take(takeObject);
        }

        public void Packaging()
        {
            if (isPackaging)
            {
                Debug.Log("포장 완료");
                return;
            }
            isPackaging = true;

            Debug.Log("포장 중!!!");
        }
    }
}