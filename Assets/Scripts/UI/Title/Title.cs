using MSKim.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSKim.UI
{
    public class Title : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private TitleView view;

        private void Start()
        {
            view.Initialize(this);
        }

        public void OnStartEvent()
        {
            Managers.Title.TitleGuest.Clear();
            Managers.Title.TitleCar.Clear();
            SceneManager.LoadScene(1);
        }

        public void OnShopEvent()
        {
            var shopPopup = Managers.Pool.GetPoolObject("ShopPopupUI");
            if (shopPopup == null) return;

            shopPopup.transform.SetParent(view.UIRoot);
            shopPopup.transform.localScale = Vector3.one;
            shopPopup.transform.localPosition = Vector3.zero;
        }

        public void OnExitEvent()
        {

        }
    }
}