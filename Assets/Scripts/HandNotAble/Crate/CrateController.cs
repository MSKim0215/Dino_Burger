using MSKim.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.HandNotAble
{
    public class CrateController : InterActionMonoBehaviour
    {
        [Header("Crate Type")]
        [SerializeField] private Utils.CrateType crateType;

        private Dictionary<Utils.CrateType, string> cratePrefabNameDict = new();

        public Utils.CrateType CrateType => crateType;

        private void Start()
        {
            for (int i = 0; i < Enum.GetValues(typeof(Utils.CrateType)).Length - 1; i++)
            {
                var type = (Utils.CrateType)i;
                cratePrefabNameDict.Add(type, $"Ingredient_{type}");
            }
        }

        public override void Take(GameObject takeObject)
        {
            if(takeObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                ingredient.Release();
            }
        }

        public override GameObject Give()
        {
            var createObj = Managers.Pool.GetPoolObject(cratePrefabNameDict[crateType]);

            if (createObj.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                ingredient.Initialize(crateType);
            }

            return createObj;
        }
    }
}