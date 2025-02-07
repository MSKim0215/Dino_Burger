using MSKim.Manager;
using System;
using UnityEngine;

namespace MSKim.UI
{
    public class MenuPanel : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var loopCount = Enum.GetValues(typeof(Utils.CrateType)).Length - 1;
            for(int i = 0; i < loopCount; i++)
            {
                var group = Managers.Pool.GetPoolObject("Menu_Group");
                if (group == null) return;

                if (group.TryGetComponent<MenuGroup>(out var menuGroup))
                {
                    if(menuGroup.transform.parent != transform)
                    {
                        menuGroup.transform.SetParent(transform);
                        menuGroup.transform.localScale = Vector3.one;
                        menuGroup.transform.localPosition = Vector3.zero;
                    }

                    menuGroup.Initialize((Utils.CrateType)i);
                }
            }
        }
    }
}