using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandAble
{
    public class FoodController : PoolAble
    {
        [Header("Food Data Info")]
        [SerializeField] protected Data.FoodData data;

        [Header("Plate")]
        [SerializeField] protected GameObject plate;

        private int currentYieldAmount;

        public int YieldAmount { get => currentYieldAmount; set => currentYieldAmount = value; }

        public Utils.FoodState CurrentFoodState { get; set; }

        public Utils.FoodType FoodType => data.Type;

        public virtual void Initialize(Utils.FoodType foodType)
        {
            data = Managers.GameData.GetFoodData(foodType);
            currentYieldAmount = data.YieldAmount;
        }

        public void Packaing(bool isActive = true)
        {
            plate.SetActive(isActive);
        }

        public override void Release()
        {
            Packaing(false);
            base.Release();
        }
    }
}