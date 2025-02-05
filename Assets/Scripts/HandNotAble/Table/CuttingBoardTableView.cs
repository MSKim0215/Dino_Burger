using System;

namespace MSKim.HandNotAble.UI
{
    [Serializable]
    public class CuttingBoardTableView : TableView
    {
        private CuttingBoardTableController controller;

        public override void Initialize<T>(T controller)
        {
            this.controller = controller as CuttingBoardTableController;
            if (this.controller == null) return;

            this.controller.OnSetUpActiveEvent(gaugeCanvas.SetActiveRoot);
            this.controller.OnSetUpValueEvent(gaugeCanvas.SetSliderValue);
        }
    }
}