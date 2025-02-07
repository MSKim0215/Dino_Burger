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

        [Header("Pool Settings")]
        [SerializeField] private GameObject stewFoodPrefab;

        [Header("Stew Settings")]
        [SerializeField] private List<Utils.CrateType> stewIngredientList = new();
        [SerializeField] private List<Utils.CrateType> currentIngredientList = new();

        [Header("Stew Cooking Time")]
        [SerializeField] private float currentCookTime = 0f;

        public bool IsContainIngredient(Utils.CrateType type) => currentIngredientList.Contains(type);

        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.Pot);
            name = data.Name;

            view.Initialize(this);
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            if (!stewObject.activeSelf) stewObject.SetActive(true);

            if(takeObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                if (!stewIngredientList.Contains(ingredient.IngredientType)) return;
                if (IsContainIngredient(ingredient.IngredientType)) return;

                currentIngredientList.Add(ingredient.IngredientType);
                OnTriggerInputIngredientEvent(ingredient.IngredientType);

                Destroy(base.Give());

                Boil();
            }
        }

        private async void Boil()
        {
            if (currentIngredientList.Count < stewIngredientList.Count) return;

            while(true)
            {
                bool isTimeOver = currentCookTime >= Utils.BOIL_STEW_COOK_TIME;
                OnTriggerOriginActiveEvent(!isTimeOver);

                currentCookTime += Time.deltaTime;
                OnTriggerValueEvent(currentCookTime / Utils.BOIL_STEW_COOK_TIME);

                await UniTask.Yield();

                if(isTimeOver)
                {
                    currentCookTime = 0f;

                    var stew = Instantiate(stewFoodPrefab);
                    stew.SetActive(false);
                    hand.SetHandUpObject(stew);
                    break;
                }
            }
        }

        public override GameObject Give()
        {
            if (hand.HandUpObject == null) return base.Give();

            OnTriggerOutputIngredientEvent();
            stewObject.SetActive(false);
            currentIngredientList.Clear();
            hand.HandUpObject.SetActive(true);
            return base.Give();
        }
    }
}