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
                else if(currentCookTime >= data.CookTime)
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