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
        [SerializeField] private Transform rootCurrency;
        [SerializeField] private List<Material> skinMatList = new();
        [SerializeField] private List<SkinnedMeshRenderer> skinRendererList = new();

        private RaycastHit handHit;
        private Ray handRay;
        private float holdPointZ;
        private bool isRelease = false;
        private int waitingNumber;
        private int orderTableNumber;
        private MSKim.UI.OrderTicket orderTicket;

        public event Action<int> OnChangeWaitingNumber;
        public event Action<int> OnChangeOrderTableNumber;
        public event Action<float> OnDelayOrderEvent;
        public event Action<bool> OnOrderBurgerCheckEvent;
        public event Action<bool> OnOrderStewCheckEvent;

        public Data.GuestData Data => data;

        public UI.GuestView View => view;

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

            if(Managers.CurrentSceneType == Utils.SceneType.Title)
            {
                CurrentWaypointType = transform.position.x > 0 ? Utils.WaypointType.Outside_L : Utils.WaypointType.Outside_R;
            }
            else
            {
                CurrentWaypointType = Utils.WaypointType.MoveStore;
            }

            currentPatientTime = 0f;

            ChangeState(ICharacterState.BehaviourState.Move);

            isRelease = false;
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
            if(Managers.CurrentSceneType == Utils.SceneType.Title)
            {
                return currentPointIndex >= Managers.TitleWaypoint.GetCurrentWaypointMaxIndex(currentWaypointType);
            }
            return (currentPointIndex >= Managers.GameWaypoint.GetCurrentWaypointMaxIndex(currentWaypointType));
        }

        public override void MovePosition()
        {
            Vector3 targetPoint;
            if (Managers.CurrentSceneType == Utils.SceneType.Title)
            {
                targetPoint = Managers.TitleWaypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            }
            else
            {
                targetPoint = Managers.GameWaypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            }

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
            Vector3 targetPoint;
            if (Managers.CurrentSceneType == Utils.SceneType.Title)
            {
                targetPoint = Managers.TitleWaypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            }
            else
            {
                targetPoint = Managers.GameWaypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            }
            
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
            Vector3 targetPoint;
            if (Managers.CurrentSceneType == Utils.SceneType.Title)
            {
                targetPoint = Managers.TitleWaypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            }
            else
            {
                targetPoint = Managers.GameWaypoint.GetCurrentWaypoint(currentWaypointType, currentPointIndex);
            }

            if (IsMoveOutside())
            {   // 거리 -> 거리 퇴장
                int maxIndex;
                if (Managers.CurrentSceneType == Utils.SceneType.Title)
                {
                    maxIndex = Managers.TitleWaypoint.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
                }
                else
                {
                    maxIndex = Managers.GameWaypoint.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
                }

                currentDistance = Mathf.Abs(targetPoint.x - transform.position.x);
                NextPointIndex(maxIndex <= currentPointIndex);
            }
            else if (IsMovePickupOutside())
            {   // 픽업 테이블 -> 거리 퇴장
                int maxIndex = Managers.GameWaypoint.GetCurrentWaypointMaxIndex(currentWaypointType) - 1;
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
                            if (!Managers.Game.Guest.IsExistWaitingGuest)
                            {
                                Managers.Game.Guest.AddPickupZone(this);
                                return;
                            }
                        }

                        if (Managers.Game.CanMoveWaitingChair)
                        {
                            Managers.Game.Guest.AddWaitingZone(this);
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
            PatientTimer();
        }

        private void UpdateWaiting()
        {
            if (WaitingNumber != 1) return;
            if (!Managers.Game.CanMovePickupTable) return;

            Managers.Game.Guest.RemoveWaitingZone();
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
                    OnOrderBurgerCheckEvent?.Invoke(isGetBurger);
                    myPickupTable.Give();
                    burger.Release();

                    int giveGoldAmount = 0;
                    for (int i = 0; i < orderBurger.Count; i++)
                    {
                        giveGoldAmount += (int)(Managers.GameData.GetIngredientData(orderBurger[i]).GuestSellPrice +
                            Managers.UserData.GetUpgradeAmount(Managers.GameData.GetIngredientData(orderBurger[i]).ItemPrice));
                    }

                    var currency = Managers.Pool.GetPoolObject("Canvas_Currency");
                    if(currency.TryGetComponent<MSKim.UI.CurrencyCanvas>(out var component))
                    {
                        component.transform.position = rootCurrency.position;
                        component.Initialize(giveGoldAmount);
                    }

                    Managers.Game.CurrentCoinAmount += giveGoldAmount;
                }
            }
        }

        private void CheckOrderStew()
        {
            if (!isOrderStew) return;
            if (isGetStew) return;
            if (myPickupTable.HandUpObject == null) return;

            if (myPickupTable.HandUpObject.TryGetComponent<HandAble.FoodController>(out var stew))
            {
                if (stew.FoodType != Utils.FoodType.Stew) return;

                isGetStew = true;
                OnOrderStewCheckEvent?.Invoke(isGetStew);
                myPickupTable.Give();
                stew.Release();

                int giveGoldAmount = 0;
                for (int i = 0; i < Managers.Game.AllowStewIncredients.Count; i++)
                {
                    var data = Managers.GameData.GetIngredientData(Managers.Game.AllowStewIncredients[i]);
                    giveGoldAmount += (int)(data.GuestSellPrice + Managers.UserData.GetUpgradeAmount(data.ItemPrice));
                }

                var currency = Managers.Pool.GetPoolObject("Canvas_Currency");
                if (currency.TryGetComponent<MSKim.UI.CurrencyCanvas>(out var component))
                {
                    component.transform.position = rootCurrency.position;
                    component.Initialize(giveGoldAmount);
                }

                Managers.Game.CurrentCoinAmount += giveGoldAmount;
            }
        }

        private void PatientTimer()
        {
            if (isRelease) return;

            if (isOrderSuccess)
            {
                ChangeState(ICharacterState.BehaviourState.OrderSuccess);
                currentPatientTime = 0f;

                if (orderTicket != null)
                {
                    orderTicket.Release();
                    orderTicket = null;
                }

                Out();
                return;
            }

            currentPatientTime += Time.deltaTime;
            OnDelayOrderEvent?.Invoke(currentPatientTime / data.Patience + Managers.UserData.GetUpgradeAmount(Utils.ShopItemIndex.SHOP_GUEST_PATIENT_TIME_INDEX));

            if (currentPatientTime >= data.Patience + Managers.UserData.GetUpgradeAmount(Utils.ShopItemIndex.SHOP_GUEST_PATIENT_TIME_INDEX))
            {
                ChangeState(ICharacterState.BehaviourState.OrderFailure);
                currentPatientTime = 0f;

                if (orderTicket != null)
                {
                    orderTicket.Release();
                    orderTicket = null;
                }

                Out();
            }
        }

        private async void Out()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            if (isRelease) return;

            Managers.Game.Guest.RemovePickupZone(this);

            if (isOrderSuccess)
            {
                ChangeState(ICharacterState.BehaviourState.MoveSuccess);
            }
            else
            {
                ChangeState(ICharacterState.BehaviourState.MoveFailure);
            }
        }

        public void Order(List<Utils.CrateType> orderBurger, bool isOrderStew)
        {
            this.orderBurger = orderBurger;
            this.isOrderStew = isOrderStew;

            FindPickupTable();

            if(!IsFindPickupTable()) return;

            CreateOrderTicket();
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
            if(Managers.CurrentSceneType == Utils.SceneType.Title)
            {
                Managers.Title.TitleGuest.Remove(gameObject);
            }
            else
            {
                Managers.Game.Guest.Remove(gameObject);
            }

            ChangeState(ICharacterState.BehaviourState.None);

            currentPointIndex = 0;
            isOrderSuccess = false;
            isGetBurger = false;
            isGetStew = false;

            isRelease = true;

            view.Release();
            base.Release();
        }
    }
}