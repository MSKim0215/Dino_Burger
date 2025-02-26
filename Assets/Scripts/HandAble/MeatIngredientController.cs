namespace MSKim.HandAble
{
    public class MeatIngredientController : IngredientController
    {
        public bool IsGrillOver { get; set; } = false;

        public override float CurrentCookTime
        {
            get => currentCookTime;
            set
            {
                currentCookTime = value;

                CheckStoveTime();
            }
        }

        private void CheckStoveTime()
        {
            if (IsGrillOver)
            {
                if (currentCookTime >= Utils.GRILL_OVERCOOKED_TIME - data.CookTime)
                {
                    ChangeCookStateObject(Utils.CookState.OverCook);
                }
            }
            else
            {
                if (currentCookTime >= data.CookTime)
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