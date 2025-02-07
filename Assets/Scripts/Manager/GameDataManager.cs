using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GameDataManager : BaseManager
    {
        [Header("Ingredient Data")]
        [SerializeField] private Data.IngredientsData ingredientDatas;

        [Header("Food Data")]
        [SerializeField] private Data.FoodsData foodDatas;

        [Header("Character Data")]
        [SerializeField] private Data.PlayersData playerDatas;
        [SerializeField] private Data.GuestsData guestDatas;

        [Header("Table Data")]
        [SerializeField] private Data.TablesData tableDatas;

        [Header("Shop Item Data")]
        [SerializeField] private Data.ShopItemsData shopItemsDatas;

        public List<Data.IngredientData> IngredientDatas => ingredientDatas.IngredientDataList;

        public Data.IngredientData GetIngredientData(Utils.CrateType type) => IngredientDatas.Find(ingredient => ingredient.Type == type);

        public List<Data.IngredientIconData> IngredientIconDatas => ingredientDatas.IngredientIconDataList;

        public Data.IngredientIconData GetIngredientIconData(Utils.CrateType type) => IngredientIconDatas.Find(ingredient => ingredient.Type == type);

        public List<Data.FoodData> FoodDatas => foodDatas.FoodDataList;

        public Data.FoodData GetFoodData(Utils.FoodType type) => FoodDatas.Find(food => food.Type == type);

        public List<Data.CharacterData> PlayerDatas => playerDatas.PlayerDataList;

        public Data.CharacterData GetPlayerData(Utils.CharacterType type) => PlayerDatas.Find(player => player.Type == type);

        public List<Data.GuestData> GuestDatas => guestDatas.GuestDataList;

        public Data.GuestData GetGuestData(Utils.CharacterType type) => GuestDatas.Find(guest => guest.Type == type);

        public List<Data.TableData> TableDatas => tableDatas.TableDataList;

        public Data.TableData GetTableData(Utils.TableType type) => TableDatas.Find(table => table.Type == type);

        public List<Data.ShopItemData> ShopItemDatas => shopItemsDatas.ShopItemDataList;

        public Data.ShopItemData GetShopItemData(int index) => ShopItemDatas.Find(item => item.Index == index);

        public List<Data.ShopItemIconData> ShopItemIconDatas => shopItemsDatas.ShopItemIconDataList;

        public Data.ShopItemIconData GetShopItemIcon(int index) => ShopItemIconDatas.Find(item => item.Index == index);

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}