using UnityEngine;

namespace MSKim.Player
{
    public class PlayerController : CharacterController
    {
        [Header("My Hand")]
        [SerializeField] private Hand hand;

        private float xAxis;
        private float zAxis;
        
        private RaycastHit handHit;
        private Ray handRay;
        private float handlingDistance = 1.5f;

        private int LayerHandAble { get => 1 << LayerMask.NameToLayer("HandAble"); }
        private int LayerHandNotAble { get => 1 << LayerMask.NameToLayer("HandNotAble"); }

        private void Start()
        {
            moveSpeed = 10f;
            rotateSpeed = 10f;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public override void Move()
        {
            xAxis = Input.GetAxis("Horizontal");
            zAxis = Input.GetAxis("Vertical");

            if (xAxis == 0f && zAxis == 0f) return;

            base.Move();
        }

        public override Vector3 GetVelocity() => new(xAxis, 0f, zAxis);

        public override void MovePosition()
        {
            transform.position += GetVelocity() * moveSpeed * Time.deltaTime;
        }

        public override void MoveRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(GetVelocity()), rotateSpeed * Time.deltaTime);
        }

        private void Update()
        {
            Pick();
            InterAction();
        }

        private void Pick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                handRay = new Ray(new Vector3(transform.position.x, 0.1f, transform.position.z), transform.forward);
                Debug.DrawLine(handRay.origin, handRay.origin + handRay.direction * handlingDistance, Color.red);

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
            if (Physics.Raycast(handRay, out handHit, handlingDistance, LayerHandNotAble))
            {
                var hitObj = handHit.collider.gameObject;
                if(hitObj != null)
                {
                    if(hitObj.TryGetComponent<HandNotAble.TableController>(out var table))
                    {
                        TableInteraction(table);
                        return;
                    }

                    if(hitObj.TryGetComponent<HandNotAble.CrateController>(out var crate))
                    {
                        CrateInteraction(crate);
                        return;
                    }
                }
                return;
            }

            hand.GetHandDown(null, new Vector3(hand.HandUpObject.transform.position.x, 0f, hand.HandUpObject.transform.position.z));
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
                                ingredient.IngredientType == Utils.CrateType.Tomato);
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
            if (Physics.Raycast(handRay, out handHit, handlingDistance, LayerHandAble + LayerHandNotAble))
            {
                var hitObj = handHit.collider.gameObject;
                if (hitObj != null)
                {
                    if(hitObj.layer == LayerMask.NameToLayer("HandNotAble"))
                    {
                        if(hitObj.TryGetComponent<HandNotAble.TableController>(out var table))
                        {
                            hand.GetHandUp(table.Give());
                            return;
                        }
                        
                        if(hitObj.TryGetComponent<HandNotAble.CrateController>(out var crate))
                        {
                            hand.GetHandUp(crate.Give());
                            return;
                        }

                        return;
                    }

                    hand.GetHandUp(hitObj);
                }
            }
        }

        private void InterAction()
        {
            if(Input.GetMouseButton(1))
            {
                handRay = new Ray(new Vector3(transform.position.x, 0.2f, transform.position.z), transform.forward);
                Debug.DrawLine(handRay.origin, handRay.origin + handRay.direction * handlingDistance, Color.red);

                if(Physics.Raycast(handRay, out handHit, handlingDistance, LayerHandNotAble))
                {
                    var hitObj = handHit.collider.gameObject;
                    if(hitObj != null)
                    {
                        if(hitObj.TryGetComponent<HandNotAble.TableController>(out var table))
                        {
                            switch(table.TableType)
                            {
                                case Utils.TableType.CuttingBoard: (table as HandNotAble.CuttingBoardTableController).Cutting(); break;
                            }
                        }
                    }
                }
            }
        }

        protected override void SettingState()
        {

        }
    }
}