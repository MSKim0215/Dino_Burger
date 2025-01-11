using UnityEngine;

namespace MSKim.HandAble
{
    public class IngredientController : MonoBehaviour
    {
        [Header("Ingredient Type")]
        [SerializeField] private Utils.CrateType ingredientType;

        public void Initialize(Utils.CrateType ingredientType)
        {
            this.ingredientType = ingredientType;
        }
    }
}