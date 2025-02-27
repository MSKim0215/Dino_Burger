using MSKim.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class CarController : CharacterController
    {
        [Header("Car Data Info")]
        [SerializeField] private Data.CarData data;

        public void Initialize(Utils.CarType carType)
        {
            data = Managers.GameData.GetCarData(carType);
        }

        public override void MovePosition()
        {
            throw new System.NotImplementedException();
        }

        public override void MoveRotation()
        {
            throw new System.NotImplementedException();
        }

        private void OnEnable()
        {
            SetModel();
        }

        private void SetModel()
        {
            //HashSet<int> exclude = new() { 6, 7 };
            //var range = Enumerable.Range(0, skinMatList.Count).Where(index => !exclude.Contains(index));
            //var rand = new System.Random();
            //int index = rand.Next(0, skinMatList.Count - exclude.Count);

            //for (int i = 0; i < skinRendererList.Count; i++)
            //{
            //    skinRendererList[i].material = skinMatList[index];
            //}
        }
    }
}