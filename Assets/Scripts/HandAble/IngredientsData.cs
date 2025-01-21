using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "IngredientsData", menuName = "GameData/Ingredient")]
    public class IngredientsData : ScriptableObject
    {
        public TextAsset Json;
        public List<IngredientData> IngredientDataList = new();
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
}