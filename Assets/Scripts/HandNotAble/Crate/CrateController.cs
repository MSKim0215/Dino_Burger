using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CrateController : MonoBehaviour, IInterAction
    {
        [Header("Crate Type")]
        [SerializeField] private Utils.CrateType crateType;

        [Header("Pool Settings")]
        [SerializeField] private HandAble.IngredientController ingredientPrefab;
        [SerializeField] private Transform ingredientRoot;

        public Utils.CrateType CrateType => crateType;

        public void Take(GameObject takeObject) => Destroy(takeObject);

        public GameObject Give()
        {
            var ingredient = Instantiate(ingredientPrefab);
            ingredient.Initialize(crateType);
            return ingredient.gameObject;
        }
    }
}