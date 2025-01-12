using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandAble
{
    public class BurgerFoodController : MonoBehaviour
    {
        [SerializeField] private GameObject burgerBottom;
        [SerializeField] private GameObject burgerTop;
        [SerializeField] private List<GameObject> ingredientList = new();

        private Dictionary<Utils.CrateType, float> correctionHeightDict = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            correctionHeightDict.Add(Utils.CrateType.Cheese, 0.06f);
            correctionHeightDict.Add(Utils.CrateType.Onion, 0.06f);
            correctionHeightDict.Add(Utils.CrateType.Lettuce, 0.05f);

            float height = burgerBottom.GetComponent<Renderer>().bounds.size.y;

            for(int i = 0; i < ingredientList.Count; i++)
            {
                var ingredient = ingredientList[i].GetComponent<IngredientController>();
                if(ingredient.IngredientType == Utils.CrateType.Cheese)
                {
                    height -= correctionHeightDict[ingredient.IngredientType];
                }

                ingredientList[i].transform.localPosition += new Vector3(0, height, 0);
                height += ingredientList[i].GetComponentInChildren<Renderer>().bounds.size.y;

                if(ingredient.IngredientType == Utils.CrateType.Lettuce || ingredient.IngredientType == Utils.CrateType.Onion)
                {
                    height -= correctionHeightDict[ingredient.IngredientType];
                }
            }

            burgerTop.transform.localPosition += new Vector3(0, height, 0);
        }

        // Update is called once per frame
        void Update()
        {   

        }
    }
}