using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CrateController : InterActionMonoBehaviour
    {
        [Header("Crate Type")]
        [SerializeField] private Utils.CrateType crateType;

        [Header("Pool Settings")]
        [SerializeField] private HandAble.IngredientController ingredientPrefab;
        [SerializeField] private Transform ingredientRoot;

        public Utils.CrateType CrateType => crateType;

        public override void Take(GameObject takeObject) => Object.Destroy(takeObject);

        public override GameObject Give()
        {
            var ingredient = Object.Instantiate(ingredientPrefab);
            ingredient.Initialize(crateType);
            return ingredient.gameObject;
        }
    }
}