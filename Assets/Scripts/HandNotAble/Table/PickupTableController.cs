using MSKim.Manager;

namespace MSKim.HandNotAble
{
    public class PickupTableController : TableController
    {
        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.Pickup);
            name = data.Name;
        }
    }
}