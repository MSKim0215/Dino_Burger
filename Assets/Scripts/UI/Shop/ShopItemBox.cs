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

        public void OnUpgrade()
        {
            if (data == null) return;

            var purchasePopup = Managers.Pool.GetPoolObject("PurchasePopupUI");
            if (purchasePopup == null) return;

            purchasePopup.transform.SetParent(GameObject.Find("TitleCanvas").transform);
            purchasePopup.transform.localScale = Vector3.one;
            purchasePopup.transform.localPosition = Vector3.zero;

            if (purchasePopup.TryGetComponent<PurchasePopup>(out var controller))
            {
                controller.Initailize(data);
            }
        }

        private void HandleChangeUpgrade(Utils.ShopItemIndex type, int level)
        {
            if(data.Index != (int)type) return;
            view.SetLevelText(level);
        }
    }
}