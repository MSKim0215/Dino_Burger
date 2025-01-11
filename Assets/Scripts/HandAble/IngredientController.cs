using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandAble
{
    public class IngredientController : MonoBehaviour
    {
        [Header("Ingredient Type")]
        [SerializeField] private Utils.CrateType ingredientType;

        [Header("Ingredient Objects")]
        [SerializeField] private GameObject[] ingredientObjects;
        
        private Dictionary<Utils.CrateType, GameObject> ingredientDict = new();

        public void Initialize(Utils.CrateType ingredientType)
        {
            InitializeDict();

            this.ingredientType = ingredientType;
            name = this.ingredientType.ToString();

            ingredientDict[this.ingredientType].SetActive(true);
        }

        private void InitializeDict()
        {
            for(int i = 0; i < ingredientObjects.Length; i++)
            {
                var type = (Utils.CrateType)Enum.Parse(typeof(Utils.CrateType), ingredientObjects[i].name);
                ingredientDict.Add(type, ingredientObjects[i]);
            }
        }
    }
}