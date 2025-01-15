using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandAble
{
    public class IngredientController : MonoBehaviour
    {
        [Header("Ingredient Info")]
        [SerializeField] private Utils.CrateType ingredientType;
        [SerializeField] protected int yieldAmount;

        [Header("Ingredient State Objects")]
        [SerializeField] private Utils.IngredientState ingredientState = Utils.IngredientState.Basic;
        [SerializeField] private GameObject[] ingredientStateObjects;

        [Header("Cooking Time")]
        [SerializeField] protected float currentCookTime;
        [SerializeField] protected float maximumCookTime;

        [Header("Other Component")]
        [SerializeField] private Renderer ingredientRenderer;
        [SerializeField] private Collider hitbox;

        private Dictionary<Utils.CookState, GameObject> ingredientCookStateDict = new();

        public virtual int YieldAmount { get => yieldAmount; set => yieldAmount = value; }

        public virtual float CurrentCookTime { get => currentCookTime; set => currentCookTime = value; }

        public float MaximumCookTime => maximumCookTime;

        public Utils.CrateType IngredientType => ingredientType;

        public Utils.IngredientState IngredientState => ingredientState;

        public Collider HitBox => hitbox;

        public float RendererHeight => ingredientRenderer.bounds.size.y;

        public void Initialize(Utils.CrateType ingredientType)
        {
            this.ingredientType = ingredientType;
            name = this.ingredientType.ToString();

            SetYieldAmount();
            InitializeCookState();
        }

        private void SetYieldAmount()
        {
            yieldAmount = ingredientType switch
            {
                Utils.CrateType.Cheese => Utils.CHEESE_INCREDIENT_YIELD,
                Utils.CrateType.Lettuce => Utils.LETTUCE_INCREDIENT_YIELD,
                Utils.CrateType.Onion => Utils.ONION_INCREDIENT_YIELD,
                Utils.CrateType.Tomato => Utils.TOMATO_INCREDIENT_YIELD,
                _ => 1,
            };
        }

        protected virtual void InitializeCookState()
        {
            for(int i = 0; i < ingredientStateObjects.Length; i++)
            {
                ingredientCookStateDict.Add((Utils.CookState)i, ingredientStateObjects[i]);
            }

            CurrentCookTime = 0f;
        }

        protected void ChangeCookStateObject(Utils.CookState targetState)
        {
            foreach(var ingredient in ingredientCookStateDict)
            {
                if (ingredient.Value.activeSelf && ingredient.Key == targetState) continue;
                ingredient.Value.SetActive(ingredient.Key == targetState);
            }
        }

        public void SetIngredientState(Utils.IngredientState targetState)
        {
            if (targetState == ingredientState) return;
            ingredientState = targetState;
        }
    }
}