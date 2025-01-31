using MSKim.Manager;
using UnityEngine;

namespace MSKim.Scene
{
    public abstract class BaseScene : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        protected abstract void Initialize();
    }
}