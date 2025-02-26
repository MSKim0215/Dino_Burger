using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class GasStoveTableController : TableControllerUseUI
    {
        [Header("Table View")]
        [SerializeField] private UI.TableView view;

        private bool isGrill = false;

        protected override void Initialize()
        {
            view.Initialize(this);
        }

        public override void Take(GameObject takeObject)
        {
            base.Take(takeObject);

            isGrill = true;
        }

        public override GameObject Give()
        {
            OnTriggerOriginActiveEvent(false);
            return base.Give();
        }

        private void Update()
        {
            if (!isGrill) return;

            Grill();
        }

        private void Grill()
        {
            if (hand.HandUpObject == null) return;

            var ingredient = hand.GetHandUpComponent<HandAble.MeatIngredientController>();
            if (ingredient == null) return;

            ingredient.CurrentCookTime += Time.deltaTime;

            float maximum = !ingredient.IsGrillOver ?
                ingredient.CurrentCookTime / ingredient.MaximumCookTime :
                ingredient.CurrentCookTime / (Utils.GRILL_OVERCOOKED_TIME - ingredient.MaximumCookTime);
            OnTriggerValueEvent(maximum);

            bool isTimeOver = !ingredient.IsGrillOver ?
                ingredient.CurrentCookTime >= ingredient.MaximumCookTime :
                ingredient.CurrentCookTime >= (Utils.GRILL_OVERCOOKED_TIME - ingredient.MaximumCookTime);

            if (hand.HandUpObject == null) return;

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
                if (ingredient.IsGrillOver) return;

                ingredient.IsGrillOver = true;
                ingredient.CurrentCookTime = 0f;

                OnTriggerOriginActiveEvent(!ingredient.IsGrillOver);
                OnTriggerChangeActiveEvent(ingredient.IsGrillOver);
            }

            ingredient.SetIngredientState(isTimeOver ? Utils.IngredientState.GrillOver : Utils.IngredientState.Grilling);
        }
    }
}