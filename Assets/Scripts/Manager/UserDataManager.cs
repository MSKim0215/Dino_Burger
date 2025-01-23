using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class UserDataManager : MonoBehaviour
    {
        private static UserDataManager instance;

        private Dictionary<Utils.GoodsType, int> userData = new();

        [Header("InGame Data Info")]
        [SerializeField] private int currentGoldAmount = 0;

        public static UserDataManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        public int CurrentGoldAmount
        {
            get => currentGoldAmount;
            set
            {
                currentGoldAmount = value;
            }
        }

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void IncreaseAmount(Utils.GoodsType type, int addAmount)
        {
            if (!userData.ContainsKey(type)) return;

            userData[type] += addAmount;
        }

        public void DecreaseAmount(Utils.GoodsType type, int subAmount)
        {
            if(!userData.ContainsKey(type)) return;

            userData[type] -= subAmount;
        }
    }
}