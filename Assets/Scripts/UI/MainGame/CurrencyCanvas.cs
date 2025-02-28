using UnityEngine;

namespace MSKim.UI
{
    public class CurrencyCanvas : PoolAble
    {
        [Header("Currency Canvas View")]
        [SerializeField] private CurrencyCanvasView view;

        private bool isInit = false;
        private float currentTime = 0f;
        private float maxTime = 2f;

        public void Initialize(int currencyAmount)
        {
            view.SetPriceText(currencyAmount);

            isInit = true;
        }

        private void Update()
        {
            if (!isInit) return;

            view.MenuGroup.transform.position += Vector3.up * Time.deltaTime;

            currentTime += Time.deltaTime;

            if(maxTime <= currentTime)
            {
                currentTime = 0f;
                Release();
            }
        }

        public override void Release()
        {
            isInit = false;

            base.Release();
        }
    }
}