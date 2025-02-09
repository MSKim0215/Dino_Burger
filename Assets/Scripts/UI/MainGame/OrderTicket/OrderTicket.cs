using MSKim.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.UI
{
    public class OrderTicket : PoolAble
    {
        [Header("OrderTicket View")]
        [SerializeField] private OrderTicketView view;

        private List<OrderGroupController> groupList = new();
        [SerializeField] private Transform groupRoot;

        public bool IsOrderStew { get; private set; }
        public int TableNumber { get; private set; }

        public void Initialize(NonPlayer.GuestController orderTarget)
        {
            IsOrderStew = orderTarget.IsOrderStew;
            TableNumber = orderTarget.OrderTableNumber;

            orderTarget.OnDelayOrderEvent -= view.SetTimer;
            orderTarget.OnDelayOrderEvent += view.SetTimer;

            CreateGroup(orderTarget.OrderBurger);

            view.Initialize(this);
        }

        private void CreateGroup(List<Utils.CrateType> ingredients)
        {
            for(int i = 0; i < (IsOrderStew ? 2 : 1); i++)
            {
                var group = Managers.Pool.GetPoolObject("Group_Ingredient");
                if (group.transform.parent != groupRoot)
                {
                    group.transform.SetParent(groupRoot);
                    group.transform.localScale = Vector3.one;
                    group.transform.localPosition = Vector3.zero;
                }

                if (group.TryGetComponent<OrderGroupController>(out var controller))
                {
                    controller.Initalize((Utils.FoodType)i, ingredients);
                    groupList.Add(controller);
                }
            }
        }

        public override void Release()
        {
            for(int i = 0; i < groupList.Count; i++)
            {
                groupList[i].Release();
            }
            groupList.Clear();

            base.Release();
        }
    }
}