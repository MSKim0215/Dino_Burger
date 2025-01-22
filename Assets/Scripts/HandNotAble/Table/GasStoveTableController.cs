using Cysharp.Threading.Tasks;
using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class GasStoveTableController : TableController
    {
        protected override void Initialize()
        {
            data = GameDataManager.Instance.GetTableData(Utils.TableType.GasStove);
            name = data.Name;
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);
            Grill();
        }

        private async void Grill()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.MeatIngredientController>();
            while(ingredient != null && hand.HandUpObject != null)
            {
                ingredient.CurrentCookTime += Time.deltaTime;

                await UniTask.Yield();

                if(ingredient.CurrentCookTime >= Utils.GRILL_OVERCOOKED_TIME)
                {
                    Debug.Log($"{ingredient.IngredientType}이 탔습니다.");
                    break;
                }
                else if(ingredient.CurrentCookTime >= ingredient.MaximumCookTime)
                {
                    Debug.Log($"{ingredient.IngredientType}이 잘 구워졌습니다.");
                    ingredient.SetIngredientState(Utils.IngredientState.GrillOver);
                }

                ingredient.SetIngredientState(Utils.IngredientState.Grilling);
            }
        }
    }
}