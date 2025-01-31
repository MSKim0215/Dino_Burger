using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class TrashcanTableController : TableController
    {
        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.TrashCan);
            name = data.Name;
        }

        public override void Take(GameObject takeObject)
        {
            Destroy(takeObject);
        }
    }
}