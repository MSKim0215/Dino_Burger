using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandAble
{
    public abstract class FoodController : MonoBehaviour
    {
        [Header("Food Data Info")]
        [SerializeField] protected Data.FoodData data;

        public Utils.FoodState CurrentFoodState { get; set; }

        public virtual void Initialize(Utils.FoodType foodType)
        {
            data = GameDataManager.Instance.GetFoodData(foodType);
            name = data.Name;
        }
    }
}