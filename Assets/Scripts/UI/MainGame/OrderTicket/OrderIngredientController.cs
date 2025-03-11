using UnityEngine;


namespace MSKim.UI
{
    public class OrderIngredientController : PoolAble
    {
        [Header("Order View")]
        [SerializeField] private OrderIngredientView view;

        private int count;

        public Utils.CrateType IngredientType { get; set; }

        public int Count
        {
            get => count;
            set
            {
                count = value;
                view.SetCountText(count);
            }
        }

        public void Initialize(Utils.CrateType ingredientType)
        {
            IngredientType = ingredientType;
            Count = 1;

            view.Initialize(this);
        }
    }
}