using MSKim.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.UI
{
    public class ShopPopup : PoolAble
    {
        [Header("UI Settings")]
        [SerializeField] private ShopView view;

        private Utils.ShopTabType currentTabType = Utils.ShopTabType.Ingredient;

        private void Start()
        {
            Managers.UserData.OnChangeCurrency -= HandleChangeCurrency;
            Managers.UserData.OnChangeCurrency += HandleChangeCurrency;

            view.Initialize(this);
            CreateItemBox(currentTabType);
        }

        private void CreateItemBox(Utils.ShopTabType tabType)
        {
            var currentItemDatas = Managers.GameData.ShopItemDatas.FindAll(itemBox => itemBox.Type == tabType);
            if (currentItemDatas == null) return;

            ReleaseItemBox();
            CreateItemBox(currentItemDatas);
        }

        private void ReleaseItemBox()
        {
            for (int i = 0; i < view.ShopItemBoxRoot.childCount; i++)
            {
                var itemBox = view.ShopItemBoxRoot.GetChild(i).gameObject;
                if (!itemBox.activeSelf) continue;

                if (itemBox.TryGetComponent<ShopItemBox>(out var shopItemBox))
                {
                    shopItemBox.Release();
                }
            }
        }

        private void CreateItemBox(List<Data.ShopItemData> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                var itemBox = ObjectPoolManager.Instance.GetPoolObject("ShopItemBox");
                if(itemBox.transform.parent != view.ShopItemBoxRoot)
                {
                    itemBox.transform.SetParent(view.ShopItemBoxRoot);
                    itemBox.transform.localScale = Vector3.one;
                    itemBox.transform.localPosition = Vector3.zero;
                }
                
                itemBox.transform.SetSiblingIndex(dataList[i].Index);

                if (itemBox.TryGetComponent<ShopItemBox>(out var shopItemBox))
                {
                    shopItemBox.Initialize(dataList[i]);
                }
            }
        }

        public void OnExitEvent()
        {
            Release();
        }

        public void OnTabEvent(Utils.ShopTabType tagetTabType)
        {
            if (currentTabType == tagetTabType) return;

            currentTabType = tagetTabType;
            view.ChangeTabButton(currentTabType);

            CreateItemBox(currentTabType);
        }

        private void HandleChangeCurrency(Utils.CurrencyType currencyType, int amount)
        {
            view.SetCurrencyText(amount);
        }

        public override void Release()
        {
            OnTabEvent(Utils.ShopTabType.Ingredient);
            base.Release();
        }
    }
}