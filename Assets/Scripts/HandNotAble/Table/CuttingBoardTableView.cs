using System;
using TMPro;
using UnityEngine;

namespace MSKim.HandNotAble.UI
{
    [Serializable]
    public class CuttingBoardTableView : TableView
    {
        [Serializable]
        public class CountCanvas : CanvasGroup
        {
            [SerializeField] private TextMeshProUGUI countText;

            public void SetCountText(int amount)
            {
                SetActiveRoot(amount > 0);
                countText.text = amount.ToString();
            }
        }

        [SerializeField] private CountCanvas countCanvas = null;

        public override void Initialize<T>(T controller)
        {
            base.Initialize(controller);
        }

        public void SetCountText(int amount) => countCanvas.SetCountText(amount);
    }
}