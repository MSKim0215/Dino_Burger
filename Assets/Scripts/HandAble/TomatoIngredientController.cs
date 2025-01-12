namespace MSKim.HandAble
{
    public class TomatoIngredientController : IngredientController
    {
        public override float CurrentCookTime
        {
            get => currentCookTime;
            set
            {
                currentCookTime = value;

                if (currentCookTime >= maximumCookTime)
                {
                    ChangeCookStateObject(Utils.CookState.Cook);
                }
                else
                {
                    ChangeCookStateObject(Utils.CookState.UnCook);
                }
            }
        }

        protected override void InitializeCookState()
        {
            maximumCookTime = Utils.CUTTING_TOMATO_COOK_TIME;

            base.InitializeCookState();
        }
    }
}