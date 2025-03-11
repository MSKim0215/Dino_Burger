using System.Collections.Generic;
using System;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "CarsData", menuName = "GameData/Car")]
    public class CarsData : BaseGameData
    {
        public List<CarData> CarDataList = new();
    }

    [Serializable]
    public class CarData : CharacterData
    {
        public Utils.CarType CarType;
        public float BreakForce;
    }
}