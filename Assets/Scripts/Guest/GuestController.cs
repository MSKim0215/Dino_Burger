using MSKim.Manager;
using System;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : CharacterController
    {
        private enum BehaviourState
        {
            Idle, Walk
        }

        [Header("Behaviour State")]
        [SerializeField] private BehaviourState currBehaviourState;

        [Header("Waypoint Settings")]
        [SerializeField] private Utils.WaypointType currentWaypointType;
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private float currentDistance = 0f;
        [SerializeField] private float checkDistance = 0.5f;

        private float holdPointZ;

        public Utils.WaypointType CurrentWaypointType
        {
            get => currentWaypointType;
            set
            {
                currentWaypointType = value;
                currentPointIndex = 0;
            }
        }

        public void Initialize()
        {
            moveSpeed = 1.5f;
            rotateSpeed = 5f;
            checkDistance = 0.8f;

            holdPointZ = transform.position.z;
            currBehaviourState = BehaviourState.Walk;
            CurrentWaypointType = Utils.WaypointType.MoveStore;
        }

        private void FixedUpdate()
        {
            switch(currBehaviourState)
            {
                case BehaviourState.Walk: FixedUpdateWalk(); break;
            }
        }

        private void FixedUpdateWalk()
        {
            if (currentPointIndex >= WaypointManager.Instance.GetCurrentWaypointMaxIndex(currentWaypointType)) return;

            base.Move();
            CheckDistance();
        }

        public override void MovePosition()
        {
            var targetPoint = WaypointManager.Instance.GetCurrentWaypoint(currentWaypointType, currentPointIndex);

            if (currentWaypointType == Utils.WaypointType.Outside_L || currentWaypointType == Utils.WaypointType.Outside_R)
            {
                MoveHoldZPosition(targetPoint);
                return;
            }

            if (currentWaypointType == Utils.WaypointType.MoveStore)
            {
                if(currentPointIndex == 0)
                {
                    MoveHoldZPosition(targetPoint);
                    return;
                }
            }

            MovePosition(targetPoint);
        }

        private void MovePosition(Vector3 targetPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        }

        private void MoveHoldZPosition(Vector3 targetPoint)
        {
            MovePosition(new(targetPoint.x, targetPoint.y, holdPointZ));
        }

        public override void MoveRotation()
        {
            var targetPoint = WaypointManager.Instance.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            
            if (currentWaypointType == Utils.WaypointType.Outside_L || currentWaypointType == Utils.WaypointType.Outside_R ||
                (currentWaypointType == Utils.WaypointType.MoveStore && currentPointIndex == 0))
            {
                targetPoint = new(targetPoint.x, targetPoint.y, holdPointZ);
            }

            var direction = targetPoint - transform.position;
            direction.Normalize();

            var rotationFixedY = CalculateRotationY(transform.rotation, direction);
            var rotationY = rotationFixedY;

            if (rotationY * rotationFixedY > 0f)
            {
                Quaternion rotationAmount = Quaternion.Euler(0f, rotateSpeed * rotationFixedY * Time.deltaTime, 0f);
                rotationY -= rotationFixedY * Time.deltaTime;
                transform.rotation *= rotationAmount;
            }
        }

        private float CalculateRotationY(Quaternion now, Vector3 targetDirection)
        {
            float seta = (90 - now.eulerAngles.y) / 180 * Mathf.PI;
            float x = Mathf.Cos(seta);
            float z = Mathf.Sin(seta);

            float inner = targetDirection.x * x + targetDirection.z * z;
            float outer = targetDirection.x * z - targetDirection.z * x;

            float delta1 = (Mathf.Acos(inner) * 180) / Mathf.PI;
            float delta2 = (Mathf.Asin(outer) * 180) / Mathf.PI;

            return (delta2 >= 0) ? delta1 : -delta1;
        }

        private void CheckDistance()
        {
            var targetPoint = WaypointManager.Instance.GetCurrentWaypoint(currentWaypointType, currentPointIndex);

            if (currentWaypointType == Utils.WaypointType.Outside_L || currentWaypointType == Utils.WaypointType.Outside_R)
            {
                currentDistance = Mathf.Abs(targetPoint.x - transform.position.x);
                if(currentDistance <= checkDistance)
                {
                    currentPointIndex++;

                    if(currentPointIndex >= WaypointManager.Instance.GetCurrentWaypointMaxIndex(currentWaypointType))
                    {
                        Release();
                    }
                }
                return;
            }

            if(currentWaypointType == Utils.WaypointType.MoveStore)
            {
                currentDistance = currentPointIndex == 0 ?
                    Mathf.Abs(targetPoint.x - transform.position.x) :
                    Vector3.Distance(transform.position, targetPoint);

                if (currentDistance <= checkDistance)
                {
                    if(currentPointIndex == 0)
                    {
                        var rand = UnityEngine.Random.Range(0, 2);  // 0: store, 1: out

                        if (!GameManager.Instance.CanMoveWaitingChair ||
                            rand == 0)
                        {
                            CurrentWaypointType = UnityEngine.Random.Range(0, 2) == 0 ? Utils.WaypointType.Outside_R : Utils.WaypointType.Outside_L;
                            return;
                        }
                    }

                    currentPointIndex++;

                    if (currentPointIndex >= WaypointManager.Instance.GetCurrentWaypointMaxIndex(currentWaypointType))
                    {
                        if(GameManager.Instance.CanMovePickupTable)
                        {
                            GameManager.Instance.AddPickupZone(this);
                        }
                        else
                        {
                            if(GameManager.Instance.CanMoveWaitingChair)
                            {
                                GameManager.Instance.AddWaitingZone(this);
                                checkDistance = 0.01f;
                            }
                        }
                    }
                }
                return;
            }

            currentDistance = Vector3.Distance(transform.position, targetPoint);
            if (currentDistance <= checkDistance)
            {
                currentPointIndex++;
            }
        }

        public override void Release()
        {
            currentPointIndex = 0;
            base.Release();
        }
    }
}