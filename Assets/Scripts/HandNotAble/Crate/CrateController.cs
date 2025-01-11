using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CrateController : MonoBehaviour, IInterAction
    {
        [Header("Crate Type")]
        [SerializeField] private Utils.CrateType crateType;

        [Header("Pool Settings")]
        [SerializeField] private GameObject ingredientPrefab;
        [SerializeField] private Transform ingredientRoot;

        public GameObject Give()
        {
            GameObject ingredient = Instantiate(ingredientPrefab);
            return ingredient;
        }

        public void Take(GameObject takeObject)
        {
            Destroy(takeObject);
        }
    }
}