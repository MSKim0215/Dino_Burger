using System.Collections.Generic;
using UnityEngine;
using MSKim.Manager;

namespace MSKim.HandAble
{
    public class IngredientController : PoolAble
    {
        [Header("Ingredient Data Info")]
        [SerializeField] protected Data.IngredientData data;

        [Header("Ingredient State Objects")]
        [SerializeField] private Utils.IngredientState ingredientState = Utils.IngredientState.Basic;
        [SerializeField] private GameObject[] ingredientStateObjects;

        [Header("Cooking Time")]
        [SerializeField] protected float currentCookTime;

        [Header("Other Component")]
        [SerializeField] private Renderer ingredientRenderer;
        [SerializeField] private Collider hitbox;

        private Dictionary<Utils.CookState, GameObject> ingredientCookStateDict = new();

        private int currentYieldAmount;

        public int YieldAmount { get => currentYieldAmount; set => currentYieldAmount = value; }

        public virtual float CurrentCookTime
        {
            get => currentCookTime;
            set
            {
                currentCookTime = value;

                if (IngredientType == Utils.CrateType.Bun || IngredientType == Utils.CrateType.None) return;

                CheckCuttingTime();
            }
        }

        private void CheckCuttingTime()
        {
            if(currentCookTime >= data.CookTime)
            {
                ChangeCookStateObject(Utils.CookState.OverCook);
            }
            else if (currentCookTime > 0 && currentCookTime < data.CookTime)
            {
                ChangeCookStateObject(Utils.CookState.Cook);
            }
            else
            {
                ChangeCookStateObject(Utils.CookState.UnCook);
            }
        }

        public float MaximumCookTime => data.CookTime;

        public Utils.CrateType IngredientType => data.Type;

        public Utils.IngredientState IngredientState => ingredientState;

        public Collider HitBox => hitbox;

        public float RendererHeight => ingredientRenderer.bounds.size.y;

        public void Initialize(Utils.CrateType ingredientType)
        {
            data = Managers.GameData.GetIngredientData(ingredientType);
            currentYieldAmount = (int)(data.YieldAmount + Managers.UserData.GetUpgradeAmount(data.ItemYield));

            InitializeCookState();
        }

        protected virtual void InitializeCookState()
        {
            for(int i = 0; i < ingredientStateObjects.Length; i++)
            {
                var cookState = (Utils.CookState)i;
                if(!ingredientCookStateDict.ContainsKey(cookState))
                {
                    ingredientCookStateDict.Add((Utils.CookState)i, ingredientStateObjects[i]);
                }
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