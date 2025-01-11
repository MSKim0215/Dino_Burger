using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class GasStoveTableController : TableController
    {
        private const float GRILL_GOOD_TIME = 5f;
        private const float GRILL_OVERCOOKED_TIME = 8f;

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);
            Grill();
        }

        private async void Grill()
        {
            var ingredient = hand.GetHandUpIngredient() as HandAble.MeatIngredientController;
            while(ingredient != null && hand.HandUpObject != null)
            {
                ingredient.CurrentCookTime += Time.deltaTime;

                await UniTask.Yield();

                if(ingredient.CurrentCookTime >= GRILL_OVERCOOKED_TIME)
                {
                    Debug.Log($"{ingredient.IngredientType}이 탔습니다.");
                    break;
                }
                else if(ingredient.CurrentCookTime >= GRILL_GOOD_TIME)
                {
                    Debug.Log($"{ingredient.IngredientType}이 잘 구워졌습니다.");
                }
            }
        }
    }
}