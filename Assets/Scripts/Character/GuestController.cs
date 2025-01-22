using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : CharacterController
    {
        [Header("Guest Data Info")]
        [SerializeField] private Data.GuestData data;

        [Header("Waypoint Settings")]
        [SerializeField] private Utils.WaypointType currentWaypointType;
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private float currentDistance = 0f;
        [SerializeField] private float checkDistance = 0.5f;

        [Header("Info Viewer")]
        [SerializeField] private bool isOrderSuccess = false;
        [SerializeField] private float currentPatientTime = 0f;
        [SerializeField] private HandNotAble.TableController myPickupTable;
        [SerializeField] private List<Utils.CrateType> orderBurger = new();
        [SerializeField] private bool isOrderStew = false;
        [SerializeField] private bool isGetBurger = false;
        [SerializeField] private bool isGetStew = false;

        private RaycastHit handHit;
        private Ray handRay;
        private float holdPointZ;

        public Data.GuestData Data => data;

        private int LayerHandAble { get => 1 << LayerMask.NameToLayer("HandAble"); }
        private int LayerHandNotAble { get => 1 << LayerMask.NameToLayer("HandNotAble"); }

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
                { ICharacterState.BehaviourState.Pickup, new PickupState() },
                { ICharacterState.BehaviourState.Order, new OrderState() },
            };
        }

        public void Initialize()
        {
            data = GameDataManager.Instance.GetGuestData(Utils.CharacterType.NPC);
            name = data.Name;

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
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, data.MoveSpeed * Time.deltaTime);
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
                Quaternion rotationAmount = Quaternion.Euler(0f, data.RotateSpeed * rotationFixedY * Time.deltaTime, 0f);
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
                        ChangeState(ICharacterState.BehaviourState.Order);
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
                case ICharacterState.BehaviourState.Order: UpdateOrder(); break;
            }
        }

        private void UpdateOrder()
        {
            if (isOrderSuccess) return;

            CheckOrderFood();
        }

        private void UpdateWaiting()
        {
            if (WaitingNumber != 1) return;
            if (!GameManager.Instance.CanMovePickupTable) return;

            GameManager.Instance.RemoveWaitingZone();
            ChangeState(ICharacterState.BehaviourState.Move);
        }

        private void CheckOrderFood()
        {
            if (myPickupTable == null) return;
            if (myPickupTable.IsHandEmpty) return;

            CheckOrderBurger();
            CheckOrderStew();

            isOrderSuccess = isGetBurger && (!isOrderStew || isGetStew);
        }

        private void CheckOrderBurger()
        {
            if (isGetBurger) return;

            if (myPickupTable.HandUpObject.TryGetComponent<HandAble.BurgerFoodController>(out var burger))
            {
                if (Enumerable.SequenceEqual(burger.GetCurrentIncredients().OrderBy(e => e), orderBurger.OrderBy(e => e)))
                {
                    isGetBurger = true;
                    Destroy(myPickupTable.Give());
                }
            }
        }

        private void CheckOrderStew()
        {
            if (!isOrderStew) return;
            if (isGetStew) return;

            if (myPickupTable.HandUpObject.TryGetComponent<HandAble.StewFoodController>(out var stew))
            {
                isGetStew = true;
                Destroy(myPickupTable.Give());
            }
        }

        public async void Order(List<Utils.CrateType> orderBurger, bool isOrderStew)
        {
            this.orderBurger = orderBurger;
            this.isOrderStew = isOrderStew;

            FindPickupTable();

            if(!IsFindPickupTable())
            {
                Debug.LogWarning("픽업테이블 못찾음");
                return;
            }
            
            while (true)
            {
                if(isOrderSuccess)
                {
                    Debug.Log("주문한거 받음");
                    currentPatientTime = 0f;
                    break;
                }

                currentPatientTime += Time.deltaTime;

                await UniTask.Yield();

                if(currentPatientTime >= data.Patience)
                {
                    Debug.Log("주문한거 못받음");
                    currentPatientTime = 0f;
                    break;
                }
            }

            GameManager.Instance.RemovePickupZone(this);
            ChangeState(ICharacterState.BehaviourState.Move);
        }

        private void FindPickupTable()
        {
            handRay = new Ray(new Vector3(transform.position.x, 0.1f, transform.position.z), transform.forward);
            Debug.DrawLine(handRay.origin, handRay.origin + handRay.direction * data.HandLength, Color.blue);

            if (Physics.Raycast(handRay, out handHit, data.HandLength, LayerHandNotAble))
            {
                var hitObj = handHit.collider.gameObject;
                if (hitObj != null)
                {
                    hitObj.TryGetComponent(out myPickupTable);
                }
            }
        }

        private bool IsFindPickupTable() => myPickupTable != null;

        public override void Release()
        {
            currentPointIndex = 0;
            isOrderSuccess = false;
            isGetBurger = false;
            isGetStew = false;
            base.Release();
        }
    }
}