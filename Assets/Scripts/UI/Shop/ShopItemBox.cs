using MSKim.Manager;
using UnityEngine;

namespace MSKim.UI
{
    public class ShopItemBox : PoolAble
    {
        [Header("Data Info")]
        [SerializeField] private Data.ShopItemData data;

        [Header("UI Settings")]
        [SerializeField] private ShopItemBoxView view;

        public Data.ShopItemData Data => data;

        public void Initialize(Data.ShopItemData data)
        {
            this.data = data;

            Managers.UserData.OnChangeUpgrade -= HandleChangeUpgrade;
            Managers.UserData.OnChangeUpgrade += HandleChangeUpgrade;

            view.Initialize(this);
        }

        public void OnUpgradeEvent()
        {
            if (data == null) return;

            Debug.Log($"{name} 업그레이드 버튼 누름");

            Managers.UserData.Payment(Utils.CurrencyType.Gold, data);
        }

        private void HandleChangeUpgrade(Utils.ShopItemIndex type, int level)
        {
            if(data.Index != (int)type) return;
            view.SetLevelText(level);
        }
    }
}