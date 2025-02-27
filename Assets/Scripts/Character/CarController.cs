using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class CarController : CharacterController
    {
        [Header("Car Data Info")]
        [SerializeField] private Data.CarData data;

        [Header("Info Viewer")]
        [SerializeField] private MeshFilter skinMeshFilter;

        [Header("Waypoint Settings")]
        [SerializeField] private Utils.CarWaypointType currentWaypointType;
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private float currentDistance = 0f;
        [SerializeField] private float checkDistance = 0.5f;

        [Header("Wheel Settings")]
        [SerializeField] private WheelCollider frontRight;
        [SerializeField] private WheelCollider frontLeft;
        [SerializeField] private WheelCollider backRight;
        [SerializeField] private WheelCollider backLeft;
        [SerializeField] private Transform frontRightTransform;
        [SerializeField] private Transform frontLeftTransform;
        [SerializeField] private Transform backRightTransform;
        [SerializeField] private Transform backLeftTransform;

        private float currentAcceleration = 0f;
        private float currentBreakForce = 0f;

        private Vector3 targetPoint;
        private int maxIndex;

        private bool isStop = false;
        private bool isRelease = false;

        private int LayerWall { get => 1 << LayerMask.NameToLayer("Wall"); }

        public Utils.CarWaypointType CurrentWaypointType
        {
            get => currentWaypointType;
            set
            {
                currentWaypointType = value;
                currentPointIndex = 0;
            }
        }

        public void Initialize(Utils.CarType carType, Utils.CarWaypointType pointType)
        {
            data = Managers.GameData.GetCarData(carType);
            skinMeshFilter.mesh = Managers.Game.Car.MeshDict[carType];
            CurrentWaypointType = pointType;

            targetPoint = Managers.Waypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            maxIndex = Managers.Waypoint.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;

            isStop = false;
            isRelease = false;
        }

        private async void FixedUpdate()
        {
            if (isRelease) return;
            if (isStop)
            {
                currentBreakForce = data.BreakForce;
                currentAcceleration = 0f;

                await UniTask.Delay(TimeSpan.FromSeconds(0.4f));

                currentBreakForce = 0f;

                isStop = false;
                return;
            }

            base.Move();
            CheckDistance();
        }

        public override void MovePosition()
        {
            currentAcceleration = data.MoveSpeed;

            frontRight.motorTorque = currentAcceleration;
            frontLeft.motorTorque = currentAcceleration;

            frontRight.brakeTorque = currentBreakForce;
            frontLeft.brakeTorque = currentBreakForce;
            backRight.brakeTorque = currentBreakForce;
            backLeft.brakeTorque = currentBreakForce;

            UpdateWheel(frontRight, frontRightTransform);
            UpdateWheel(frontLeft, frontLeftTransform);
            UpdateWheel(backRight, backRightTransform);
            UpdateWheel(backLeft, backLeftTransform);
        }

        private void UpdateWheel(WheelCollider wheelCol, Transform wheelTrans)
        {
            Vector3 position;
            Quaternion rotation;

            wheelCol.GetWorldPose(out position, out rotation);
            wheelTrans.position = position;
            wheelTrans.rotation = rotation;
        }

        public override void MoveRotation() { }

        private void CheckDistance()
        {
            currentDistance = Mathf.Abs(targetPoint.x - transform.position.x);
            NextPointIndex(maxIndex <= currentPointIndex);
        }

        private void NextPointIndex(bool isRelease = false)
        {
            if (currentDistance <= checkDistance)
            {
                currentPointIndex++;

                if (isRelease)
                {
                    Release();
                }
            }
        }

        private void Update()
        {
            Look();
        }

        private void Look()
        {
            if (isStop) return;

            Vector3 origin = transform.position + transform.forward; // 자동차의 앞부분 기준
            if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, data.HandLength, LayerWall))
            {
                isStop = true;
            }

            Debug.DrawRay(origin, transform.forward * data.HandLength, Color.red);
        }

        public override void Release()
        {
            Managers.Game.Car.Remove(gameObject);

            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;

            frontRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;
            backLeft.brakeTorque = 0;

            currentPointIndex = 0;
            isStop = false;
            isRelease = true;

            base.Release();
        }
    }
}