using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "GuestsData", menuName = "GameData/Guest")]
    public class GuestsData : BaseGameData
    {
        public List<GuestData> GuestDataList = new();
    }

    [Serializable]
    public class GuestData : CharacterData
    {
        public int MinimumToppingCount;
        public int MaximumToppingCount;
        public float Patience;
    }
}