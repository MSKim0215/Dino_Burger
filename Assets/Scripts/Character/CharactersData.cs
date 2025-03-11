using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "PlayersData", menuName = "GameData/Player")]
    public class PlayersData : BaseGameData
    {
        public List<CharacterData> PlayerDataList = new();
    }

    [Serializable]
    public class CharacterData
    {
        public string Name;
        public Utils.CharacterType Type;
        public float MoveSpeed;
        public float RotateSpeed;
        public float HandLength;
        public float ViewAngle;
    }
}