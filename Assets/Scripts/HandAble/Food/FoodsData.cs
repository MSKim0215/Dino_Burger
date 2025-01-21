using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "FoodsData", menuName = "GameData/Food")]
    public class FoodsData : BaseGameData
    {
        public List<FoodData> FoodDataList = new();
    }

    [Serializable]
    public class FoodData
    {
        public string Name;
        public Utils.FoodType Type;
        public int YieldAmount;
        public float CookTime;
    }
}