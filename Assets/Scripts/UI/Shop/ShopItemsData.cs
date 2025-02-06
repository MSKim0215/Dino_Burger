using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "ShopItemsData", menuName = "GameData/ShopItem")]
    public class ShopItemsData : BaseGameData
    {
        public List<ShopItemData> ShopItemDataList = new();
        public List<ShopItemIconData> ShopItemIconDataList = new();
    }

    [Serializable]
    public class ShopItemData
    {
        public string Name;
        public Utils.ShopTabType Type;
        public int Index;
        public int Price;
        public int BaseLevel = 1;
        public int MaximumLevel;
        public float UpgradeAmount;
    }

    [Serializable]
    public class ShopItemIconData
    {
        public int Index;
        public Sprite Icon;
    }
}