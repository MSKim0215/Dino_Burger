using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : CharacterController
    {
        [Header("Guest Data Info")]
        [SerializeField] private Data.GuestData data;

        [Header("Guest View")]
        [SerializeField] private UI.GuestView view;

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

        [SerializeField] private List<Material> skinMatList = new();
        [SerializeField] private List<SkinnedMeshRenderer> skinRendererList = new();

        private RaycastHit handHit;
        private Ray handRay;
        private float holdPointZ;

        private int waitingNumber;
        private int orderTableNumber;
        private MSKim.UI.OrderTicket orderTicket;

        public event Action<int> OnChangeWaitingNumber;
        public event Action<int> OnChangeOrderTableNumber;
        public event Action<float> OnDelayOrderEvent;

        public Data.GuestData Data => data;

        public UI.GuestView View => view;

        private int LayerHandAble { get => 1 << LayerMask.NameToLayer("HandAble"); }
        private int LayerHandNotAble { get => 1 << LayerMask.NameToLayer("HandNotAble"); }

        public bool IsOrderStew => isOrderStew;

        public List<Utils.CrateType> OrderBurger => orderBurger;

        public int WaitingNumber
        {
            get => waitingNumber;
            set
            {
                waitingNumber = value;
                OnChangeWaitingNumber?.Invoke(waitingNumber);
            }
        }

        public int OrderTableNumber
        {
            get => orderTableNumber;
            set
            {
                orderTableNumber = value;
                OnChangeOrderTableNumber?.Invoke(orderTableNumber);
            }
        }

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
            state = new(this);

            stateDict = new()
            {
                { ICharacterState.BehaviourState.Move, new MoveState() },
                { ICharacterState.BehaviourState.Waiting, new WaitingState() },
                { ICharacterState.BehaviourState.Order, new OrderState() },
                { ICharacterState.BehaviourState.OrderSuccess, new OrderSuccessState() },
                { ICharacterState.BehaviourState.OrderFailure, new OrderFailureState() },
                { ICharacterState.BehaviourState.MoveSuccess, new MoveSuccessState() },
                { ICharacterState.BehaviourState.MoveFailure, new MoveFailureState() },
                { ICharacterState.BehaviourState.None, new NoneState() }
            };
        }

        public void Initialize()
        {
            data = Managers.GameData.GetGuestData(Utils.CharacterType.NPC);
            view.Initialize(this);

            holdPointZ = transform.position.z;
            CurrentWaypointType = Utils.WaypointType.MoveStore;

            ChangeState(ICharacterState.BehaviourState.Move);
        }

        private void SetGuestModel()
        {
            HashSet<int> exclude = new() { 6, 7 };
            var range = Enumerable.Range(0, skinMatList.Count).Where(index => !exclude.Contains(index));
            var rand = new System.Random();
            int index = rand.Next(0, skinMatList.Count - exclude.Count);

            for (int i = 0; i < skinRendererList.Count; i++)
            {
                skinRendererList[i].material = skinMatList[index];
            }
        }

        private void FixedUpdate()
        {
            if (state.CurrentState.Get() != ICharacterState.BehaviourState.Move &&
                state.CurrentState.Get() != ICharacterState.BehaviourState.MoveSuccess &&
                state.CurrentState.Get() != ICharacterState.BehaviourState.MoveFailure) return;

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
            return (currentPointIndex >= Managers.Waypoint.GetCurrentWaypointMaxIndex(currentWaypointType));
        }

        public override void MovePosition()
        {
            var targetPoint = Managers.Waypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);

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
            var targetPoint = Managers.Waypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            
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
            var targetPoint = Managers.Waypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);

            if (IsMoveOutside())
            {   // 거리 -> 거리 퇴장
                int maxIndex = Managers.Waypoint.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
                currentDistance = Mathf.Abs(targetPoint.x - transform.position.x);
                NextPointIndex(maxIndex <= currentPointIndex);
            }
            else if (IsMovePickupOutside())
            {   // 픽업 테이블 -> 거리 퇴장
                int maxIndex = Managers.Waypoint.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
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

                        if (!Managers.Game.CanMoveWaitingChair ||
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

                        if (Managers.Game.CanMovePickupTable)
                        {
                            if (!Managers.Game.IsExistWaitingGuest)
                            {
                                Managers.Game.AddPickupZone(this);
                                return;
                            }
                        }

                        if (Managers.Game.CanMoveWaitingChair)
                        {
                            Managers.Game.AddWaitingZone(this);
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
            if (state.CurrentState.Get() == ICharacterState.BehaviourState.Move ||
                state.CurrentState.Get() == ICharacterState.BehaviourState.MoveFailure ||
                state.CurrentState.Get() == ICharacterState.BehaviourState.MoveSuccess) return;

            switch(state.CurrentState.Get())
            {
                case ICharacterState.BehaviourState.Waiting: UpdateWaiting(); break;
                case ICharacterState.BehaviourState.Order: UpdateOrder(); break;
            }
        }

        private void UpdateOrder()
        {
            if (isOrderSuccess) return;
            if (state.CurrentState.Get() != ICharacterState.BehaviourState.Order) return;

            CheckOrderFood();
        }

        private void UpdateWaiting()
        {
            if (WaitingNumber != 1) return;
            if (!Managers.Game.CanMovePickupTable) return;

            Managers.Game.RemoveWaitingZone();
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

            if(!IsFindPickupTable()) return;

            CreateOrderTicket();

            while (true)
            {
                if(isOrderSuccess)
                {
                    ChangeState(ICharacterState.BehaviourState.OrderSuccess);
                    currentPatientTime = 0f;

                    int giveGoldAmount = 0;
                    for (int i = 0; i < orderBurger.Count; i++)
                    {
                        giveGoldAmount += Managers.GameData.GetIngredientData(orderBurger[i]).GuestSellPrice;
                    }

                    if(isOrderStew)
                    {
                        giveGoldAmount += Managers.GameData.GetFoodData(Utils.FoodType.Stew).GuestSellPrice;
                    }

                    Managers.UserData.CurrentGoldAmount += giveGoldAmount;

                    if (orderTicket != null)
                    {
                        orderTicket.Release();
                        orderTicket = null;
                    }

                    break;
                }

                currentPatientTime += Time.deltaTime;
                OnDelayOrderEvent?.Invoke(currentPatientTime / data.Patience);

                await UniTask.Yield();

                if(currentPatientTime >= data.Patience)
                {
                    ChangeState(ICharacterState.BehaviourState.OrderFailure);
                    currentPatientTime = 0f;

                    if (orderTicket != null)
                    {
                        orderTicket.Release();
                        orderTicket = null;
                    }

                    break;
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            Managers.Game.RemovePickupZone(this);

            if(isOrderSuccess)
            {
                ChangeState(ICharacterState.BehaviourState.MoveSuccess);
            }
            else
            {
                ChangeState(ICharacterState.BehaviourState.MoveFailure);
            }
        }

        private void CreateOrderTicket()
        {
            var createTicket = Managers.Pool.GetPoolObject("OrderTicket");
            var root = GameObject.Find("OrderTickets").transform;

            if (createTicket.transform.parent != root)
            {
                createTicket.transform.SetParent(root);
                createTicket.transform.localScale = Vector3.one;
                createTicket.transform.localPosition = Vector3.zero;
            }

            if (createTicket.TryGetComponent<MSKim.UI.OrderTicket>(out var orderTicket))
            {
                this.orderTicket = orderTicket;
                this.orderTicket.Initialize(this);
            }
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

        private void OnEnable()
        {
            SetGuestModel();
        }

        public override void Release()
        {
            ChangeState(ICharacterState.BehaviourState.None);

            currentPointIndex = 0;
            isOrderSuccess = false;
            isGetBurger = false;
            isGetStew = false;

            view.Release();
            base.Release();
        }
    }
}