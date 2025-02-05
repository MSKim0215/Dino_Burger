using System;

namespace MSKim.HandNotAble.UI
{
    [Serializable]
    public class CuttingBoardTableView : TableView
    {
        public override void Initialize(TableController controller)
        {
            if (controller is not CuttingBoardTableController) return;

            var cutting = controller as CuttingBoardTableController;
            cutting.OnActiveEvent -= gaugeCanvas.SetActiveRoot;
            cutting.OnActiveEvent += gaugeCanvas.SetActiveRoot;
            cutting.OnValueEvent -= gaugeCanvas.SetSliderValue;
            cutting.OnValueEvent += gaugeCanvas.SetSliderValue;
        }
    }
}