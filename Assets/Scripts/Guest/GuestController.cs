using MSKim.Manager;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : CharacterController
    {
        [Header("Waypoint Settings")]
        [SerializeField] private Utils.WaypointType currentWaypointType;
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private float currentDistance = 0f;
        [SerializeField] private float checkDistance = 0.5f;
        [SerializeField] private float testTimer = 0f;
        private float testTimerMax = 30f;

        private float holdPointZ;

        public int WaitingNumber { get; set; }

        public Utils.WaypointType CurrentWaypointType
        {
            get => currentWaypointType;
            set
            {
                currentWaypointType = value;
                currentPointIndex = 0;
            }
        }

        protected override void SettingState()
        {
            stateDict = new()
            {
                { ICharacterState.BehaviourState.Move, new MoveState() },
                { ICharacterState.BehaviourState.Waiting, new WaitingState() },
                { ICharacterState.BehaviourState.Pickup, new PickupState() }
            };
        }

        public void Initialize()
        {
            moveSpeed = 1.5f;
            rotateSpeed = 5f;
            checkDistance = 0.8f;

            holdPointZ = transform.position.z;
            CurrentWaypointType = Utils.WaypointType.MoveStore;

            ChangeState(ICharacterState.BehaviourState.Move);
        }

        private void FixedUpdate()
        {
            if (state.CurrentState.Get() != ICharacterState.BehaviourState.Move) return;

            FixedUpdateWalk();
        }

        private void FixedUpdateWalk()
        {
            if(IsAtLastWaypoint()) return;

            base.Move();
            CheckDistance();
        }

        private bool IsAtLastWaypoint()
        {
            return (currentPointIndex >= WaypointManager.Instance.GetCurrentWaypointMaxIndex(currentWaypointType));
        }

        public override void MovePosition()
        {
            var targetPoint = WaypointManager.Instance.GetCurrentWaypoint(currentWaypointType, currentPointIndex);

            if(ShouldHoldZPosition())
            {
                MoveHoldZPosition(targetPoint);
                return;
            }

            MovePosition(targetPoint);
        }

        private bool ShouldHoldZPosition()
        {
            return (((currentWaypointType == Utils.WaypointType.Pickup_Outside_L || currentWaypointType == Utils.WaypointType.Pickup_Outside_R) && currentPointIndex == 2) ||
                (currentWaypointType == Utils.WaypointType.Outside_L || currentWaypointType == Utils.WaypointType.Outside_R) ||
                (currentWaypointType == Utils.WaypointType.MoveStore && currentPointIndex == 0));
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
                (currentWaypointType == Utils.WaypointType.MoveStore && currentPointIndex == 0) ||
                ((currentWaypointType == Utils.WaypointType.Pickup_Outside_L || currentWaypointType == Utils.WaypointType.Pickup_Outside_R) && currentPointIndex == 2))
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

        private bool IsMoveOutside()
        {
            return currentWaypointType == Utils.WaypointType.Outside_L || currentWaypointType == Utils.WaypointType.Outside_R;
        }

        private bool IsMovePickupOutside()
        {
            return currentWaypointType == Utils.WaypointType.Pickup_Outside_L || currentWaypointType == Utils.WaypointType.Pickup_Outside_R;
        }

        private bool IsMoveStore()
        {
            return currentWaypointType == Utils.WaypointType.MoveStore;
        }

        private void NextPointIndex(bool isRelease = false)
        {
            if(currentDistance <= checkDistance)
            {
                currentPointIndex++;

                if (isRelease)
                {
                    Release();
                }
            }
        }

        private float GetDistanceWithPointIndex(int baseIndex, Vector3 targetPoint)
        {
            return currentPointIndex == baseIndex ?
                Mathf.Abs(targetPoint.x - transform.position.x) : Vector3.Distance(transform.position, targetPoint);
        }

        private void CheckDistance()
        {
            var targetPoint = WaypointManager.Instance.GetCurrentWaypoint(currentWaypointType, currentPointIndex);

            if (IsMoveOutside())
            {   // 거리 -> 거리 퇴장
                int maxIndex = WaypointManager.Instance.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
                currentDistance = Mathf.Abs(targetPoint.x - transform.position.x);
                NextPointIndex(maxIndex <= currentPointIndex);
            }
            else if (IsMovePickupOutside())
            {   // 픽업 테이블 -> 거리 퇴장
                int maxIndex = WaypointManager.Instance.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
                currentDistance = GetDistanceWithPointIndex(maxIndex, targetPoint);
                NextPointIndex(maxIndex <= currentPointIndex);
            }
            else if (IsMoveStore())
            {   // 거리 -> 가게 안으로 이동
                currentDistance = GetDistanceWithPointIndex(0, targetPoint);

                if (currentDistance <= checkDistance)
                {
                    if (currentPointIndex == 0)
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

                    if (IsAtLastWaypoint())
                    {
                        checkDistance = 0.01f;

                        if (GameManager.Instance.CanMovePickupTable)
                        {
                            if (!GameManager.Instance.IsExistWaitingGuest)
                            {
                                GameManager.Instance.AddPickupZone(this);
                                return;
                            }
                        }

                        if (GameManager.Instance.CanMoveWaitingChair)
                        {
                            GameManager.Instance.AddWaitingZone(this);
                            return;
                        }
                    }
                }
            }
            else
            {
                currentDistance = Vector3.Distance(transform.position, targetPoint);
                if (currentDistance <= checkDistance)
                {
                    currentPointIndex++;

                    if (currentWaypointType.ToString().Contains("Pickup"))
                    {
                        ChangeState(ICharacterState.BehaviourState.Pickup);
                    }
                    else if (currentWaypointType.ToString().Contains("Waiting"))
                    {
                        ChangeState(ICharacterState.BehaviourState.Waiting);
                    }
                }
            }
        }

        private void Update()
        {
            if (state.CurrentState.Get() == ICharacterState.BehaviourState.Move) return;

            switch(state.CurrentState.Get())
            {
                case ICharacterState.BehaviourState.Waiting: UpdateWaiting(); break;
                case ICharacterState.BehaviourState.Pickup: UpdatePickup(); break;
            }
        }

        private void UpdatePickup()
        {
            testTimer += Time.deltaTime;

            if(testTimer > testTimerMax)
            {
                testTimer = 0f;
                GameManager.Instance.RemovePickupZone(this);
                ChangeState(ICharacterState.BehaviourState.Move);
            }
        }

        private void UpdateWaiting()
        {
            if (WaitingNumber != 1) return;
            if (!GameManager.Instance.CanMovePickupTable) return;

            GameManager.Instance.RemoveWaitingZone();
            ChangeState(ICharacterState.BehaviourState.Move);
        }

        public override void Release()
        {
            currentPointIndex = 0;
            base.Release();
        }
    }
}