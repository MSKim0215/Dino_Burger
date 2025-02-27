using MSKim.Manager;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class CarController : CharacterController
    {
        [Header("Car Data Info")]
        [SerializeField] private Data.CarData data;

        [Header("Info Viewer")]
        [SerializeField] private MeshFilter skinMeshFilter;

        public void Initialize(Utils.CarType carType)
        {
            data = Managers.GameData.GetCarData(carType);
            skinMeshFilter.mesh = Managers.Game.Car.MeshDict[carType];
        }

        public override void MovePosition()
        {
            throw new System.NotImplementedException();
        }

        public override void MoveRotation()
        {
            throw new System.NotImplementedException();
        }
    }
}