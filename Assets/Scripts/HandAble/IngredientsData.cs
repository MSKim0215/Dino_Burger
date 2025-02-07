using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "IngredientsData", menuName = "GameData/Ingredient")]
    public class IngredientsData : BaseGameData
    {
        public List<IngredientData> IngredientDataList = new();
        public List<IngredientIconData> IngredientIconDataList = new();
    }

    [Serializable]
    public class IngredientData
    {
        public string Name;
        public Utils.CrateType Type;
        public int YieldAmount;
        public float CookTime;
        public int ShopBuyPrice;
        public int GuestSellPrice;
    }

    [Serializable]
    public class IngredientIconData
    {
        public Utils.CrateType Type;
        public Sprite Icon;
    }
}