namespace MSKim.HandAble
{
    public class OnionIngredientController : IngredientController
    {
        public override float CurrentCookTime
        {
            get => currentCookTime;
            set
            {
                currentCookTime = value;

                if (currentCookTime >= data.CookTime)
                {
                    ChangeCookStateObject(Utils.CookState.OverCook);
                }
                else if (currentCookTime > 0 && currentCookTime < data.CookTime)
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