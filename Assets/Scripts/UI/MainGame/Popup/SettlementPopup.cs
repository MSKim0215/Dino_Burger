using UnityEngine;

namespace MSKim.UI
{
    public class SettlementPopup : PoolAble
    {
        [Header("Settlement View")]
        [SerializeField] private SettlementPopupView view;

        public void Initialize()
        {
            view.Initialize(this);
        }
    }
}