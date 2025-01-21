using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        private static GameDataManager instance;

        [Header("Ingredient Data")]
        [SerializeField] private Data.IngredientsData ingredientDatas;

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