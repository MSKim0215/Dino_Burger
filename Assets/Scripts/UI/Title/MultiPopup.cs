using MSKim.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSKim.UI
{
    public class MultiPopup : PoolAble
    {
        [Header("UI Settings")]
        [SerializeField] private MultiView view;

        private void Start()
        {
            view.Initialize(this);
        }

        public void OnBackEvent()
        {
            Release();
        }

        public void OnMultiEvent()
        {
            Managers.Title.TitleGuest.Clear();
            Managers.Title.TitleCar.Clear();
            SceneManager.LoadScene(2);
        }

        public void OnLoadingStartEvent() => view.ActiveLoading();

        public void OnLoadingEndEvent() => view.UnActiveLoading();
    }
}