using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GameDataManager : BaseManager
    {
        [Header("GameData List")]
        [SerializeField] private List<BaseGameData> gameDataList = new();

        private readonly Dictionary<Utils.GameDataIndex, BaseGameData> gameDataDict = new();

        private List<T> GetDataList<T, TData>(Utils.GameDataIndex index, Func<TData, List<T>> selector) where TData : BaseGameData
        {
            if (gameDataDict.TryGetValue(index, out var data))
            {
                return selector(data as TData) ?? new();
            }
            return new();
        }

        private List<Data.IngredientData> IngredientDatas => 
            GetDataList<Data.IngredientData, Data.IngredientsData>
                (Utils.GameDataIndex.IngredientsData, data => data?.IngredientDataList);

        private List<Data.IngredientIconData> IngredientIconDatas =>
            GetDataList<Data.IngredientIconData, Data.IngredientsData>
                (Utils.GameDataIndex.IngredientsData, data => data?.IngredientIconDataList);

        private List<Data.FoodData> FoodDatas =>
            GetDataList<Data.FoodData, Data.FoodsData>
                (Utils.GameDataIndex.FoodsData, data => data?.FoodDataList);

        private List<Data.CharacterData> PlayerDatas =>
            GetDataList<Data.CharacterData, Data.PlayersData>
                (Utils.GameDataIndex.PlayersData, data => data?.PlayerDataList);

        private List<Data.GuestData> GuestDatas =>
            GetDataList<Data.GuestData, Data.GuestsData>
                (Utils.GameDataIndex.GuestsData, data => data?.GuestDataList);

        public List<Data.ShopItemData> ShopItemDatas =>
            GetDataList<Data.ShopItemData, Data.ShopItemsData>
                (Utils.GameDataIndex.ShopItemsData, data => data?.ShopItemDataList);

        private List<Data.ShopItemIconData> ShopItemIconDatas =>
            GetDataList<Data.ShopItemIconData, Data.ShopItemsData>
                (Utils.GameDataIndex.ShopItemsData, data => data?.ShopItemIconDataList);

        private List<Data.CarData> CarDatas =>
            GetDataList<Data.CarData, Data.CarsData>
                (Utils.GameDataIndex.CarsData, data => data?.CarDataList);

        public Data.IngredientData GetIngredientData(Utils.CrateType type) => IngredientDatas.Find(ingredient => ingredient.Type == type);

        public Data.IngredientIconData GetIngredientIconData(Utils.CrateType type) => IngredientIconDatas.Find(ingredient => ingredient.Type == type);

        public Data.FoodData GetFoodData(Utils.FoodType type) => FoodDatas.Find(food => food.Type == type);

        public Data.CharacterData GetPlayerData(Utils.CharacterType type) => PlayerDatas.Find(player => player.Type == type);

        public Data.GuestData GetGuestData(Utils.CharacterType type) => GuestDatas.Find(guest => guest.Type == type);

        public Data.ShopItemData GetShopItemData(int index) => ShopItemDatas.Find(item => item.Index == index);

        public Data.ShopItemIconData GetShopItemIcon(int index) => ShopItemIconDatas.Find(item => item.Index == index);

        public Data.CarData GetCarData(Utils.CarType type) => CarDatas.Find(car => car.CarType == type);

        public override void Initialize()
        {
            base.Initialize();

            SetGameData();
        }

        private void SetGameData()
        {
            foreach (var gameData in gameDataList)
            {
                var dataType = gameData.GetType();
                var field = typeof(Utils.GameDataIndex).GetFields()
                    .FirstOrDefault(f => f.Name.Equals(dataType.Name, StringComparison.OrdinalIgnoreCase));

                if (field != null)
                {
                    var index = (Utils.GameDataIndex)field.GetValue(null);
                    gameDataDict[index] = gameData; 
                }
            }
        }
    }
}