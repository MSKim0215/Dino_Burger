using MSKim.Manager;
using UnityEngine;

namespace MSKim.UI
{
    public class ShopPopup : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private ShopView view;

        private Utils.ShopTabType currentTabType = Utils.ShopTabType.Ingredient;

        private void Start()
        {
            view.Initialize(this);
            CreateItemBox();
        }

        private void CreateItemBox()
        {
            var currentItemDatas = GameDataManager.Instance.ShopItemDatas.FindAll(itemBox => itemBox.Type == currentTabType);
            if (currentItemDatas == null) return;

            for (int i = 0; i < currentItemDatas.Count; i++)
            {
                var itemBox = ObjectPoolManager.Instance.GetPoolObject("ShopItemBox");
                itemBox.transform.SetParent(view.ShopItemBoxRoot);
                itemBox.transform.localScale = Vector3.one;
                itemBox.transform.localPosition = Vector3.zero;

                if(itemBox.TryGetComponent<ShopItemBox>(out var shopItemBox))
                {
                    shopItemBox.Initialize(currentItemDatas[i]);
                }
            }
        }

        public void OnExitEvent()
        {
            Destroy(gameObject);
        }

        public void OnTabEvent(Utils.ShopTabType tagetTabType)
        {
            if (currentTabType == tagetTabType) return;

            currentTabType = tagetTabType;
            view.ChangeTabButton(currentTabType);
        }
    }
}