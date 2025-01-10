using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PackagingTableController : TableController
    {
        private bool isPackaging = false;

        public void Packaging()
        {
            if (isPackaging)
            {
                Debug.Log("포장 완료");
                return;
            }
            isPackaging = true;

            Debug.Log("포장 중!!!");
        }
    }
}