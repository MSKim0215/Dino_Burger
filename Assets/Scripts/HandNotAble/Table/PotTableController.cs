using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PotTableController : TableControllerUseUI
    {
        [Header("Table View")]
        [SerializeField] private UI.PotTableView view;

        [Header("Other Objects")]
        [SerializeField] private GameObject stewObject;

        [Header("Stew Settings")]
        [SerializeField] private List<Utils.CrateType> currentIngredientList = new();

        [Header("Stew Cooking Time")]
        [SerializeField] private float currentCookTime = 0f;

        private bool isBoil = false;

        public bool IsContainIngredient(Utils.CrateType type) => currentIngredientList.Contains(type);

        protected override void Initialize()
        {
            view.Initialize(this);
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            if (!stewObject.activeSelf) stewObject.SetActive(true);

            if(takeObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                if (!Managers.Game.AllowStewIncredients.Contains(ingredient.IngredientType)) return;
                if (IsContainIngredient(ingredient.IngredientType)) return;

                currentIngredientList.Add(ingredient.IngredientType);
                OnTriggerInputIngredientEvent(ingredient.IngredientType);

                Destroy(base.Give());

                isBoil = true;
            }
        }

        private void Update()
        {
            if (!isBoil) return;

            Boil();
        }

        private void Boil()
        {
            if (currentIngredientList.Count < Managers.Game.AllowStewIncredients.Count) return;

            bool isTimeOver = currentCookTime >= Utils.BOIL_STEW_COOK_TIME;
            OnTriggerOriginActiveEvent(!isTimeOver);

            currentCookTime += Time.deltaTime;
            OnTriggerValueEvent(currentCookTime / Utils.BOIL_STEW_COOK_TIME);

            if (isTimeOver)
            {
                currentCookTime = 0f;

                var createObj = Managers.Pool.GetPoolObject("Food_Stew");
                if (createObj.TryGetComponent<HandAble.FoodController>(out var stew))
                {
                    stew.Initialize(Utils.FoodType.Stew);
                }

                createObj.SetActive(false);
                hand.SetHandUpObject(createObj);

                OnTriggerValueCompleteEvent();

                isBoil = false;
            }
        }
            
        public override GameObject Give()
        {
            var stew = hand.GetHandUpComponent<HandAble.FoodController>();
            if (stew == null) return base.Give();
            if (stew.YieldAmount <= 1)
            {
                OnTriggerOutputIngredientEvent();
                stewObject.SetActive(false);
                currentIngredientList.Clear();
                hand.HandUpObject.SetActive(true);
                return base.Give();
            }

            var createObj = Managers.Pool.GetPoolObject("Food_Stew");
            if (createObj.TryGetComponent<HandAble.FoodController>(out var controller))
            {
                controller.Initialize(stew.FoodType);
            }

            stew.YieldAmount--;

            return createObj;
        }
    }
}