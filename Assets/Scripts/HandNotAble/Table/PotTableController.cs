using UnityEngine;

namespace MSKim.HandNotAble
{
    public class PotTableController : TableController
    {
        [Header("Other Objects")]
        [SerializeField] private GameObject stewObject;

        [Header("Pool Settings")]
        [SerializeField] private GameObject stewFoodPrefab;

        private void Update()
        {
            Boil();
        }

        private void Boil()
        {
            if (hand.HandUpObject == null) return;

            if (!stewObject.activeSelf)
            {
                stewObject.SetActive(true);
            }

            Debug.Log("끓이는 중~~");
        }

        public override GameObject Give()
        {
            if (hand.HandUpObject == null) return base.Give();

            stewObject.SetActive(false);
            hand.SetHandUpObject(Instantiate(stewFoodPrefab));
            return base.Give();
        }
    }
}