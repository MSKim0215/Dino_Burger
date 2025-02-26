using MSKim.Manager;
using UnityEngine;

namespace MSKim.HandAble
{
    public class BunIngredientController : IngredientController
    {
        public void StartCooking(HandNotAble.TableController table, GameObject playerHandUpObject)
        {
            var createObj = Managers.Pool.GetPoolObject("Food_Burger");
            if (createObj.TryGetComponent<BurgerFoodController>(out var burger))
            {
                burger.transform.localPosition = transform.localPosition;
                burger.transform.rotation = transform.rotation;

                table.Take(createObj);

                burger.Initialize(Utils.FoodType.Hamburger);
                burger.Stack(playerHandUpObject);

                Release();
            }
        }
    }
}