using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CrateController : MonoBehaviour, IBaseInterAction
    {
        [Header("Crate Type")]
        [SerializeField] private Utils.CrateType crateType;

        [Header("Pool Settings")]
        [SerializeField] private HandAble.IngredientController ingredientPrefab;
        [SerializeField] private Transform ingredientRoot;

        [Header("Outline Settings")]
        [SerializeField] private Material outline;
        [SerializeField] private Renderer renderers;
        [SerializeField] private List<Material> matList = new();

        private bool isActiveOutline = false;

        public Utils.CrateType CrateType => crateType;

        public void Take(GameObject takeObject) => Destroy(takeObject);

        public GameObject Give()
        {
            var ingredient = Instantiate(ingredientPrefab);
            ingredient.Initialize(crateType);
            return ingredient.gameObject;
        }

        public void ActiveOutline()
        {
            if (isActiveOutline) return;

            isActiveOutline = true;
            matList.Clear();
            matList.AddRange(renderers.materials);
            matList.Add(outline);

            renderers.materials = matList.ToArray();
        }

        public void UnActiveOutline()
        {
            if (!isActiveOutline) return;

            isActiveOutline = false;
            matList.Clear();
            matList.AddRange(renderers.materials);
            matList.Remove(outline);

            renderers.materials = matList.ToArray();
        }
    }
}