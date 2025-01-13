using UnityEngine;

namespace MSKim.HandAble
{
    public class BunIngredientController : IngredientController
    {
        [Header("Pool Settings")]
        [SerializeField] private BurgerFoodController burgerPrefab;

        public void StartCooking(HandNotAble.TableController table, GameObject playerHandUpObject)
        {
            var burger = Instantiate(burgerPrefab);
            burger.transform.localPosition = transform.localPosition;
            burger.transform.rotation = transform.rotation;
            var tableObject = table.Give();
            table.Take(burger.gameObject);
            Destroy(gameObject);
            Destroy(tableObject);

            burger.Initialize();
            burger.Stack(playerHandUpObject);
        }
    }
}