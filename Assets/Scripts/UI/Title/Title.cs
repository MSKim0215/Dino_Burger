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

        public void OnSingleEvent()
        {
            Managers.Title.TitleGuest.Clear();
            Managers.Title.TitleCar.Clear();
            SceneManager.LoadScene(1);
        }

        public void OnMultiEvent()
        {
            var popup = CreatePopup("MultiPopupUI");
            if (popup == null) return;

            if(popup.TryGetComponent<MultiPopup>(out var multi))
            {
                //multi.OnRefreshList();
            }
        }

        public void OnShopEvent()
        {
            CreatePopup("ShopPopupUI");
        }

        private GameObject CreatePopup(string popupName)
        {
            var popup = Managers.Pool.GetPoolObject(popupName);
            if (popup == null) return null;

            popup.transform.SetParent(view.UIRoot);
            popup.transform.localScale = Vector3.one;
            popup.transform.localPosition = Vector3.zero;

            return popup;
        }

        public void OnExitEvent()
        {
            Application.Quit();
        }
    }
}