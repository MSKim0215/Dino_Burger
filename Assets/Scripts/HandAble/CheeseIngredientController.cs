namespace MSKim.HandAble
{
    public class CheeseIngredientController : IngredientController
    {
        public override float CurrentCookTime
        {
            get => currentCookTime;
            set
            {
                currentCookTime = value;

                if (currentCookTime >= Utils.CUTTING_CHEESE_COOK_TIME)
                {
                    ChangeCookStateObject(Utils.CookState.OverCook);
                }
                else if(currentCookTime > 0 && currentCookTime < Utils.CUTTING_CHEESE_COOK_TIME)
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