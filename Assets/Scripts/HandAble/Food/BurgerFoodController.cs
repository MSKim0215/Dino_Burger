using MSKim.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.HandAble
{
    public class BurgerFoodController : FoodController
    {
        [Header("Burger View")]
        [SerializeField] private UI.BurgerView view;

        [Header("Other Object")]
        [SerializeField] private Renderer bottom;
        [SerializeField] private Renderer top;

        [Header("Current Ingredient List")]
        [SerializeField] private List<IngredientController> ingredientList = new();

        private Dictionary<Utils.CrateType, float> correctionHeightDict = new();
        private float currentHeight = 0f;

        public event Action<IngredientController> OnStackIngredientEvent;

        public float CurrentHeight
        {
            get => currentHeight;
            set
            {
                currentHeight = value;
                MoveTopPosition();
            }
        }

        public override void Initialize(Utils.FoodType foodType)
        {
            base.Initialize(foodType);

            view.Initialize(this);

            currentHeight = bottom.bounds.size.y;

            if (correctionHeightDict.Count > 0) return;

            correctionHeightDict.Add(Utils.CrateType.Cheese, 0.06f);
            correctionHeightDict.Add(Utils.CrateType.Onion, 0.05f);
            correctionHeightDict.Add(Utils.CrateType.Lettuce, 0.05f);
        }

        public void Stack(GameObject ingredientObject)
        {
            if (ingredientObject == null) return;

            if (ingredientObject.TryGetComponent<IngredientController>(out var ingredient))
            {
                if (!Managers.Game.AllowBurgerIncredients.Contains(ingredient.IngredientType)) return;

                ingredientObject.transform.SetParent(transform);
                ingredientObject.transform.localPosition = Vector3.zero;
                ingredient.HitBox.enabled = false;
                ingredientList.Add(ingredient);
                OnStackIngredientEvent?.Invoke(ingredient);

                if (ingredient.IngredientType == Utils.CrateType.Cheese)
                {
                    CurrentHeight -= correctionHeightDict[ingredient.IngredientType];
                }

                MovePosition(ingredientObject);
                CurrentHeight += ingredient.RendererHeight;

                if (ingredient.IngredientType == Utils.CrateType.Lettuce || ingredient.IngredientType == Utils.CrateType.Onion)
                {
                    CurrentHeight -= correctionHeightDict[ingredient.IngredientType];
                }
            }
        }

        private void MovePosition(GameObject target)
        {
            target.transform.localPosition += new Vector3(0f, currentHeight, 0f);
        }

        private void MoveTopPosition()
        {
            top.transform.localPosition = new Vector3(0f, currentHeight, 0f);
        }

        public List<Utils.CrateType> GetCurrentIncredients()
        {
            return ingredientList.Select(ingredient => ingredient.IngredientType).ToList();
        }

        public override void Release()
        {
            for(int i = ingredientList.Count - 1; i >= 0; i--)
            {
                ingredientList[i].Release();
            }

            ingredientList.Clear();
                
            view.Release();
            base.Release();
        }
    }
}