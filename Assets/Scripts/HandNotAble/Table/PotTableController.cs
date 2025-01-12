using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PotTableController : TableController
    {
        [Header("Other Objects")]
        [SerializeField] private GameObject stewObject;

        [Header("Pool Settings")]
        [SerializeField] private GameObject stewFoodPrefab;

        [Header("Stew Settings")]
        [SerializeField] private List<Utils.CrateType> stewIngredientList = new();
        [SerializeField] private List<Utils.CrateType> currentIngredientList = new();

        [Header("Stew Cooking Time")]
        [SerializeField] private float currentCookTime = 0f;

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            if (!stewObject.activeSelf) stewObject.SetActive(true);

            if(takeObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                if (!stewIngredientList.Contains(ingredient.IngredientType)) return;
                if (currentIngredientList.Contains(ingredient.IngredientType)) return;

                currentIngredientList.Add(ingredient.IngredientType);

                Destroy(base.Give());

                Boil();
            }
        }

        private async void Boil()
        {
            if (currentIngredientList.Count < stewIngredientList.Count) return;

            while(true)
            {
                currentCookTime += Time.deltaTime;

                await UniTask.Yield();

                if(currentCookTime >= Utils.BOIL_STEW_COOK_TIME)
                {
                    Debug.Log("Stew가 완성되었습니다.");

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

            stewObject.SetActive(false);
            hand.HandUpObject.SetActive(true);
            return base.Give();
        }
    }
}