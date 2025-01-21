using MSKim.Manager;
using UnityEngine;

public abstract class FoodController : MonoBehaviour
{
    [Header("Food Data Info")]
    [SerializeField] protected MSKim.Data.FoodData data;

    public virtual void Initialize(Utils.FoodType foodType)
    {
        data = GameDataManager.Instance.GetFoodData(foodType);
        name = data.Name;
    }
}
