using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        private static GameDataManager instance;

        [Header("Ingredient Data")]
        [SerializeField] private Data.IngredientsData ingredientDatas;

        [Header("Food Data")]
        [SerializeField] private Data.FoodsData foodDatas;

        public static GameDataManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        public List<Data.IngredientData> IngredientDatas => ingredientDatas.IngredientDataList;

        public Data.IngredientData GetIngredientData(Utils.CrateType ingredientType) => IngredientDatas.Find(ingredient => ingredient.Type == ingredientType);

        public List<Data.FoodData> FoodDatas => foodDatas.FoodDataList;

        public Data.FoodData GetFoodData(Utils.FoodType foodType) => FoodDatas.Find(food => food.Type == foodType);

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
                
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}