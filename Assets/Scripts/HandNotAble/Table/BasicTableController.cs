using MSKim.Manager;

namespace MSKim.HandNotAble
{
    public class BasicTableController : TableController
    {
        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.Basic);
            name = data.Name;
        }
    }
}