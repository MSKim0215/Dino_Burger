using MSKim.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Player
{
    public class PlayerController : CharacterController
    {
        [Header("Player Data Info")]
        [SerializeField] private Data.CharacterData data;

        [Header("Guest View")]
        [SerializeField] private CharacterView view;

        [Header("My Hand")]
        [SerializeField] private Hand hand;
        [SerializeField] private Hand toolHand;

        private HandNotAble.CuttingBoardTableController prevCuttingTable;

        private float viewAngle = 60f;
        private float viewDistance = 1.5f;
        private GameObject mostDetectedObject;
        private GameObject lastDetectedObject;
        private Dictionary<GameObject, int> detectedObjectDict = new();

        private float xAxis;
        private float zAxis;
        
        private RaycastHit handHit;
        private Ray handRay;
        private int LayerHandAble { get => 1 << LayerMask.NameToLayer("HandAble"); }
        private int LayerHandNotAble { get => 1 << LayerMask.NameToLayer("HandNotAble"); }

        public CharacterView View => view;

        protected override void SettingState()
        {
            state = new(this);

            stateDict = new()
            {
                { ICharacterState.BehaviourState.Move, new MoveState() },
                { ICharacterState.BehaviourState.Waiting, new WaitingState() },
                { ICharacterState.BehaviourState.InterAction, new InterActionState() }
            };
        }

        private void Start()
        {
            Initailize();
        }

        public void Initailize()
        {
            data = Managers.GameData.GetPlayerData(Utils.CharacterType.Player);
            name = data.Name;

            ChangeState(ICharacterState.BehaviourState.Waiting);
        }

        private void FixedUpdate()
        {
            Move();
        }

        public override void Move()
        {
            xAxis = Input.GetAxis("Horizontal");
            zAxis = Input.GetAxis("Vertical");

            if (xAxis == 0f && zAxis == 0f)
            {
                ChangeState(ICharacterState.BehaviourState.Waiting);
                return;
            }

            ChangeState(ICharacterState.BehaviourState.Move);
            base.Move();
        }

        public override Vector3 GetVelocity() => new(xAxis, 0f, zAxis);

        public override void MovePosition()
        {
            transform.position += 
                (data.MoveSpeed + Managers.UserData.GetUpgradeAmount(Utils.ShopItemIndex.SHOP_PLAYER_MOVE_SPEED_INDEX)) * Time.deltaTime * GetVelocity();
        }

        public override void MoveRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(GetVelocity()), data.RotateSpeed * Time.deltaTime);
        }

        private void Update()
        {
            Look();
            Pick();
            InterAction();
        }

        private void Look()
        {
            mostDetectedObject = GetDetectHandAbleObject();

            // 가장 많이 감지된 오브젝트가 변경되었는지 확인
            if (mostDetectedObject != lastDetectedObject)
            {
                if(lastDetectedObject != null)
                {
                    if (lastDetectedObject.TryGetComponent<InterActionMonoBehaviour>(out var interactionObj))
                    {
                        interactionObj.IsActiveHightlight = false;
                    }
                }

                lastDetectedObject = mostDetectedObject; // 현재 감지된 오브젝트를 이전 오브젝트로 업데이트
            }
        }

        private GameObject GetDetectHandAbleObject()
        {
            detectedObjectDict.Clear();

            for (float angle = -viewAngle / 2; angle < viewAngle / 2; angle += 5f)
            {
                Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
                RaycastHit hit;

                if (Physics.Raycast(new Vector3(transform.position.x, 0.1f, transform.position.z), direction, out hit, data.HandLength, LayerHandNotAble + LayerHandAble))
                {
                    var hitObj = hit.collider.gameObject;

                    // 감지된 오브젝트 카운트 증가
                    if (detectedObjectDict.ContainsKey(hitObj))
                    {
                        detectedObjectDict[hitObj]++;
                    }
                    else
                    {
                        detectedObjectDict.Add(hitObj, 1);
                    }
                }
            }

            // 감지된 오브젝트가 없으면 null 반환
            if (detectedObjectDict.Count <= 0) return null;

            GameObject mostDetected = null;
            int maxCount = 0;

            // 가장 많이 감지된 오브젝트 찾기
            foreach (var item in detectedObjectDict)
            {
                if (item.Value > maxCount)
                {
                    maxCount = item.Value;
                    mostDetected = item.Key;
                }
            }

            // 하이라이트 상태 업데이트
            foreach (var item in detectedObjectDict)
            {
                if (item.Key.TryGetComponent<InterActionMonoBehaviour>(out var interactionObj))
                {
                    interactionObj.IsActiveHightlight = item.Key == mostDetected;
                }
            }

            return mostDetected;
        }

        private void Pick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hand.HandUpObject != null)
                {
                    PickDown();
                    return;
                }

                PickUp();
            }
        }

        private void PickDown()
        {
            if (mostDetectedObject == null)
            {
                hand.GetHandDown(null, new Vector3(hand.HandUpObject.transform.position.x, 0f, hand.HandUpObject.transform.position.z));
                return;
            }

            if(mostDetectedObject.TryGetComponent<HandNotAble.TableController>(out var table))
            {
                TableInteraction(table);
                return;
            }

            if (mostDetectedObject.TryGetComponent<HandNotAble.CrateController>(out var crate))
            {
                CrateInteraction(crate);
                return;
            }
        }

        private void TableInteraction(HandNotAble.TableController table)
        {
            if(table.TableType == Utils.TableType.TrashCan)
            {
                table.Take(hand.HandUpObject);
                hand.ClearHand();
                return;
            }

            if (!table.IsHandEmpty)
            {
                if(table.IsHandUpObjectBurger())
                {
                    if (hand.HandUpObjectType != Utils.CrateType.None &&
                    hand.HandUpObjectType != Utils.CrateType.Bun)
                    {
                        if (hand.HandUpObjectState != Utils.IngredientState.None &&
                            hand.HandUpObjectState != Utils.IngredientState.Basic)
                        {
                            if (table.HandUpObject.TryGetComponent<HandAble.BurgerFoodController>(out var burger))
                            {
                                burger.Stack(hand.HandUpObject);
                                hand.ClearHand();
                            }
                        }
                    }
                    return;
                }

                if (hand.IsHandUpObjectFood()) return;
                if (table.HandUpObjectType != Utils.CrateType.Bun) return;

                if (hand.HandUpObjectType != Utils.CrateType.None &&
                    hand.HandUpObjectType != Utils.CrateType.Bun)
                {
                    if (hand.HandUpObjectState != Utils.IngredientState.None &&
                        hand.HandUpObjectState != Utils.IngredientState.Basic)
                    {
                        if (table.HandUpObject.TryGetComponent<HandAble.BunIngredientController>(out var burger))
                        {
                            burger.StartCooking(table, hand.HandUpObject);
                            hand.ClearHand();
                        }
                    }
                }
                return;
            }

            if (table.TableType == Utils.TableType.Basic || 
                (table.TableType == Utils.TableType.Packaging && hand.IsHandUpObjectFood()))
            {
                table.Take(hand.HandUpObject);
                hand.ClearHand();
                return;
            }

            if(table.TableType == Utils.TableType.Pickup)
            {
                if(hand.HandUpObject.TryGetComponent<HandAble.FoodController>(out var food))
                {
                    if (food.CurrentFoodState == Utils.FoodState.None) return;

                    table.Take(hand.HandUpObject);
                    hand.ClearHand();
                    return;
                }
            }

            if (hand.HandUpObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                bool canTake = false;

                switch (table.TableType)
                {
                    case Utils.TableType.CuttingBoard:
                        canTake = ingredient.IngredientType != Utils.CrateType.Meat;
                        break;

                    case Utils.TableType.Pot:
                        canTake = ingredient.IngredientState == Utils.IngredientState.CutOver &&
                                (ingredient.IngredientType == Utils.CrateType.Lettuce ||
                                ingredient.IngredientType == Utils.CrateType.Onion ||
                                ingredient.IngredientType == Utils.CrateType.Tomato) &&
                                !(table as HandNotAble.PotTableController).IsContainIngredient(ingredient.IngredientType);
                        break;

                    case Utils.TableType.GasStove:
                        canTake = ingredient.IngredientType == Utils.CrateType.Meat;
                        break;
                }

                if(canTake)
                {
                    table.Take(hand.HandUpObject);
                    hand.ClearHand();
                }
            }
        }

        private void CrateInteraction(HandNotAble.CrateController crate)
        {
            if (hand.HandUpObject.TryGetComponent<HandAble.IngredientController>(out var ingredient))
            {
                if (crate.CrateType == ingredient.IngredientType)
                {
                    if (ingredient.IngredientState != Utils.IngredientState.Basic) return;

                    crate.Take(hand.HandUpObject);
                    hand.ClearHand();
                }
            }
        }

        private void PickUp()
        {
            if (mostDetectedObject == null) return;

            if(mostDetectedObject.TryGetComponent<HandNotAble.TableController>(out var table))
            {
                hand.GetHandUp(table.Give());
                return;
            }

            if(mostDetectedObject.TryGetComponent<HandNotAble.CrateController>(out var crate))
            {
                hand.GetHandUp(crate.Give());
                return;
            }

            hand.GetHandUp(mostDetectedObject);
        }

        private void InterAction()
        {
            if(state.CurrentState.Get() == ICharacterState.BehaviourState.Move)
            {
                if (toolHand.HandUpObject == null) return;

                prevCuttingTable.TakeTool(toolHand.HandUpObject);
                toolHand.ClearHand();
                prevCuttingTable = null;
                return;
            }

            if(Input.GetMouseButton(1))
            {
                if (mostDetectedObject == null) return;

                if(mostDetectedObject.TryGetComponent<HandNotAble.CuttingBoardTableController>(out var table))
                {
                    if (table.IsHandEmpty) return;
                    if (table.IsCutOver)
                    {
                        if (toolHand.HandUpObject == null) return;

                        prevCuttingTable.TakeTool(toolHand.HandUpObject);
                        toolHand.ClearHand();
                        prevCuttingTable = null;
                        return;
                    }

                    prevCuttingTable = table;
                    toolHand.GetHandUpHoldRotate(table.GiveTool());
                    ChangeState(ICharacterState.BehaviourState.InterAction);
                    table.Cutting();
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (mostDetectedObject == null) return;

                if (mostDetectedObject.TryGetComponent<HandNotAble.CuttingBoardTableController>(out var table))
                {
                    prevCuttingTable = table;
                    table.TakeTool(toolHand.HandUpObject);
                    ChangeState(ICharacterState.BehaviourState.Waiting);
                    toolHand.ClearHand();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (mostDetectedObject == null) return;

            Gizmos.color = Color.red;
            Vector3 forward = transform.forward;

            // 각도에 따른 레이 그리기
            for (float angle = -viewAngle / 2; angle < viewAngle / 2; angle += 5f)
            {
                Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
                RaycastHit hit;

                // 가장 많이 감지된 오브젝트와 연결된 레이만 그리기
                if (Physics.Raycast(new Vector3(transform.position.x, 0.1f, transform.position.z), direction, out hit, data.HandLength, LayerHandNotAble + LayerHandAble))
                {
                    if (hit.collider.gameObject == mostDetectedObject)
                    {
                        Gizmos.DrawLine(transform.position, hit.point);
                    }
                }
            }
        }
    }
}