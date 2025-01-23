using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "ShopItemsData", menuName = "GameData/ShopItem")]
    public class ShopItemsData : BaseGameData
    {
        public List<ShopItemData> ShopItemDataList = new();
    }

    [Serializable]
    public class ShopItemData
    {
        public string Name;
        public int Index;
        public int Price;
        public int BaseLevel;
        public int MaximumLevel;
        public float UpgradeAmount;
    }
}