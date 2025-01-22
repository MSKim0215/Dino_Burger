using MSKim.Manager;

namespace MSKim.HandNotAble
{
    public class TrashcanTableController : TableController
    {
        protected override void Initialize()
        {
            data = GameDataManager.Instance.GetTableData(Utils.TableType.TrashCan);
            name = data.Name;
        }
    }
}