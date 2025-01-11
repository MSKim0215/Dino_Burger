using UnityEngine;

namespace MSKim.HandAble
{
    public class MeatIngredientController : IngredientController
    {
        public override float CurrentCookTime
        {
            get => currentCookTime;
            set
            {
                currentCookTime = value;

                if(currentCookTime >= Utils.GRILL_OVERCOOKED_TIME)
                {
                    ChangeCookStateObject(Utils.CookState.OverCook);
                }
                else if(currentCookTime >= Utils.GRILL_COOK_TIME)
                {
                    ChangeCookStateObject(Utils.CookState.Cook);
                }
                else
                {
                    ChangeCookStateObject(Utils.CookState.UnCook);
                }
            }
        }


    }
}