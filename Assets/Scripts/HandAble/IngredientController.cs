using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandAble
{
    public class IngredientController : MonoBehaviour
    {
        [Header("Ingredient Type")]
        [SerializeField] private Utils.CrateType ingredientType;

        [Header("Ingredient State Objects")]
        [SerializeField] private GameObject[] ingredientStateObjects;

        [Header("Cooking Time")]
        [SerializeField] protected float currentCookTime;
        [SerializeField] protected float maximumCookTime;

        private Dictionary<Utils.CookState, GameObject> ingredientCookStateDict = new();

        public virtual float CurrentCookTime { get => currentCookTime; set => currentCookTime = value; }

        public float MaximumCookTime => maximumCookTime;

        public Utils.CrateType IngredientType => ingredientType;

        public void Initialize(Utils.CrateType ingredientType)
        {
            this.ingredientType = ingredientType;
            name = this.ingredientType.ToString();

            InitializeCookState();
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
    }
}