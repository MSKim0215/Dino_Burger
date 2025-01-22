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

        [Header("Character Data")]
        [SerializeField] private Data.PlayersData playerDatas;
        [SerializeField] private Data.GuestsData guestDatas;

        [Header("Table Data")]
        [SerializeField] private Data.TablesData tableDatas;

        public static GameDataManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        public List<Data.IngredientData> IngredientDatas => ingredientDatas.IngredientDataList;

        public Data.IngredientData GetIngredientData(Utils.CrateType type) => IngredientDatas.Find(ingredient => ingredient.Type == type);

        public List<Data.FoodData> FoodDatas => foodDatas.FoodDataList;

        public Data.FoodData GetFoodData(Utils.FoodType type) => FoodDatas.Find(food => food.Type == type);

        public List<Data.CharacterData> PlayerDatas => playerDatas.PlayerDataList;

        public Data.CharacterData GetPlayerData(Utils.CharacterType type) => PlayerDatas.Find(player => player.Type == type);

        public List<Data.GuestData> GuestDatas => guestDatas.GuestDataList;

        public Data.GuestData GetGuestData(Utils.CharacterType type) => GuestDatas.Find(guest => guest.Type == type);


        public List<Data.TableData> TableDatas => tableDatas.TableDataList;

        public Data.TableData GetTableData(Utils.TableType type) => TableDatas.Find(table => table.Type == type);

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