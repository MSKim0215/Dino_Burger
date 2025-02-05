using Cysharp.Threading.Tasks;
using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class GasStoveTableController : TableControllerUseUI
    {
        [Header("Table View")]
        [SerializeField] private UI.TableView view;

        protected override void Initialize()
        {
            data = Managers.GameData.GetTableData(Utils.TableType.GasStove);
            name = data.Name;

            view.Initialize(this);
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            Grill();
        }

        public override GameObject Give()
        {
            OnTriggerOriginActiveEvent(false);
            return base.Give();
        }

        private async void Grill()
        {
            var ingredient = hand.GetHandUpComponent<HandAble.MeatIngredientController>();

            while (ingredient != null && hand.HandUpObject != null)
            {
                ingredient.CurrentCookTime += Time.deltaTime;

                float maximum = !ingredient.IsGrillOver ?
                    ingredient.CurrentCookTime / ingredient.MaximumCookTime :
                    ingredient.CurrentCookTime / (Utils.GRILL_OVERCOOKED_TIME - ingredient.MaximumCookTime);
                OnTriggerValueEvent(maximum);

                await UniTask.Yield();

                bool isTimeOver = !ingredient.IsGrillOver ?
                    ingredient.CurrentCookTime >= ingredient.MaximumCookTime :
                    ingredient.CurrentCookTime >= (Utils.GRILL_OVERCOOKED_TIME - ingredient.MaximumCookTime);

                if (hand.HandUpObject == null) break;

                if (ingredient.IsGrillOver)
                {   // 오버쿡 체크 시작
                    OnTriggerChangeActiveEvent(!isTimeOver);
                }
                else
                {   // 일반쿡 체크 시작
                    OnTriggerOriginActiveEvent(true);
                }

                if (isTimeOver)
                {
                    if (ingredient.IsGrillOver) break;

                    ingredient.IsGrillOver = true;
                    ingredient.CurrentCookTime = 0f;

                    OnTriggerOriginActiveEvent(!ingredient.IsGrillOver);
                    OnTriggerChangeActiveEvent(ingredient.IsGrillOver);
                }

                ingredient.SetIngredientState(isTimeOver ? 
                    Utils.IngredientState.GrillOver : Utils.IngredientState.Grilling);
            }
        }
    }
}