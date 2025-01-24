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