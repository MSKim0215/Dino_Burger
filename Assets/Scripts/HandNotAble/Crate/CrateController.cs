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

        [Header("Hightlight Settings")]
        [SerializeField] private Material highLight;
        [SerializeField] private Renderer renderers;
        [SerializeField] private Material[] baseMats;
        [SerializeField] private Material[] highMats;

        private bool isActiveHighlight = false;

        public bool IsActiveHightlight
        {
            get => isActiveHighlight;
            set
            {
                if (isActiveHighlight == value) return;

                isActiveHighlight = value;
                SetActiveHighlight();
            }
        }

        public Utils.CrateType CrateType => crateType;


        public void Take(GameObject takeObject) => Destroy(takeObject);

        public GameObject Give()
        {
            var ingredient = Instantiate(ingredientPrefab);
            ingredient.Initialize(crateType);
            return ingredient.gameObject;
        }

        private void SetActiveHighlight()
        {
            if (isActiveHighlight)
            {
                renderers.materials = highMats;
            }
            else
            {
                renderers.materials = baseMats;
            }
        }
    }
}