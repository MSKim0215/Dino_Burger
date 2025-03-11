using UnityEngine;

namespace MSKim.UI
{
    public class MenuGroup : PoolAble
    {
        [Header("MenuGroup View")]
        [SerializeField] private MenuGroupView view;

        public Utils.CrateType CurrentMenuType { get; private set; } = Utils.CrateType.None;

        public void Initialize(Utils.CrateType ingredientType)
        {
            CurrentMenuType = ingredientType;

            view.Initialize(this);
        }
    }
}