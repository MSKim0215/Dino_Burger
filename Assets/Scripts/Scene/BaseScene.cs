using MSKim.Manager;
using UnityEngine;

namespace MSKim.Scene
{
    public class BaseScene : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Managers.Instance.Initialize();
        }
    }
}