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

            this.controller.OnActiveEvent -= gaugeCanvas.SetActiveRoot;
            this.controller.OnActiveEvent += gaugeCanvas.SetActiveRoot;
            this.controller.OnValueEvent -= gaugeCanvas.SetSliderValue;
            this.controller.OnValueEvent += gaugeCanvas.SetSliderValue;
        }
    }
}