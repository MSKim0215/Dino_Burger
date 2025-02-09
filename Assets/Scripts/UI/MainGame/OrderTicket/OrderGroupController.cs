using MSKim.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.UI
{
    public class OrderGroupController : PoolAble
    {
        private Dictionary<Utils.CrateType, OrderIngredientController> burgerFrameDict = new();
        private Dictionary<Utils.CrateType, OrderIngredientController> stewFrameDict = new();

        public void Initalize(Utils.FoodType foodType, List<Utils.CrateType> orderIngredients)
        {
            switch(foodType)
            {
                case Utils.FoodType.Hamburger: CreateBurgerFrame(orderIngredients); break;
                case Utils.FoodType.Stew: CreateStewFrame(); break;
            }
        }

        private void CreateBurgerFrame(List<Utils.CrateType> orderIngredients)
        {
            var bun = Managers.Pool.GetPoolObject("Frame_Ingredient");
            if (bun.transform.parent != transform)
            {
                bun.transform.SetParent(transform);
                bun.transform.localScale = Vector3.one;
                bun.transform.localPosition = Vector3.zero;
            }

            if (bun.TryGetComponent<OrderIngredientController>(out var bunBox))
            {
                bunBox.Initialize(Utils.CrateType.Bun);
                burgerFrameDict.Add(Utils.CrateType.Bun, bunBox);
            }

            for (int i = 0; i < orderIngredients.Count; i++)
            {
                var ingredientType = orderIngredients[i];
                if (!burgerFrameDict.ContainsKey(ingredientType))
                {
                    var frame = Managers.Pool.GetPoolObject("Frame_Ingredient");
                    if (frame.transform.parent != transform)
                    {
                        frame.transform.SetParent(transform);
                        frame.transform.localScale = Vector3.one;
                        frame.transform.localPosition = Vector3.zero;
                    }

                    if (frame.TryGetComponent<OrderIngredientController>(out var frameBox))
                    {
                        frameBox.Initialize(orderIngredients[i]);
                        burgerFrameDict.Add(ingredientType, frameBox);
                    }
                }
                else
                {
                    burgerFrameDict[ingredientType].Count++;
                }
            }
        }

        private void CreateStewFrame()
        {
            for(int i = 2; i < 5; i++)
            {
                var frame = Managers.Pool.GetPoolObject("Frame_Ingredient");
                if (frame.transform.parent != transform)
                {
                    frame.transform.SetParent(transform);
                    frame.transform.localScale = Vector3.one;
                    frame.transform.localPosition = Vector3.zero;
                }

                if (frame.TryGetComponent<OrderIngredientController>(out var frameBox))
                {
                    frameBox.Initialize((Utils.CrateType)i);
                    stewFrameDict.Add((Utils.CrateType)i, frameBox);
                }
            }
        }

        public override void Release()
        {
            foreach(var burgerFrame in burgerFrameDict.Values)
            {
                burgerFrame.Release();
            }

            foreach(var stewFrame in stewFrameDict.Values)
            { 
                stewFrame.Release();
            }

            burgerFrameDict.Clear();
            stewFrameDict.Clear();

            base.Release();
        }
    }
}