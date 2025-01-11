using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CrateController : MonoBehaviour
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
    }
}