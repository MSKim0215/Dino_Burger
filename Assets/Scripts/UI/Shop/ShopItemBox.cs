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

            view.Initialize(this);
        }
    }
}