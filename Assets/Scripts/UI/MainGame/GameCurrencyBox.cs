using UnityEngine;

namespace MSKim.UI
{
    public class GameCurrencyBox : MonoBehaviour
    {
        [Header("CurrencyBox View")]
        [SerializeField] private GameCurrencyBoxView view;

        private void Start()
        {
            view.Initialize();
        }
    }
}