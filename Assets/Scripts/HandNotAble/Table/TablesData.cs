using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Data
{
    [CreateAssetMenu(fileName = "TablesData", menuName = "GameData/Table")]
    public class TablesData : BaseGameData
    {
        public List<TableData> TableDataList = new();
    }

    [Serializable]
    public class TableData
    {
        public string Name;
        public Utils.TableType Type;
    }
}