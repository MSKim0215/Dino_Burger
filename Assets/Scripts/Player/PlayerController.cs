using UnityEngine;

namespace MSKim.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("My Hand")]
        [SerializeField] private Hand hand;

        private float xAxis;
        private float zAxis;
        private float moveSpeed = 10f;
        private float rotateSpeed = 10f;
        
        private RaycastHit handHit;
        private Ray handRay;
        private float handlingDistance = 1.5f;

        private int LayerHandAble { get => 1 << LayerMask.NameToLayer("HandAble"); }
        private int LayerHandNotAble { get => 1 << LayerMask.NameToLayer("HandNotAble"); }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            xAxis = Input.GetAxis("Horizontal");
            zAxis = Input.GetAxis("Vertical");

            if (xAxis == 0f && zAxis == 0f) return;

            var velocity = new Vector3(xAxis, 0f, zAxis);
            transform.position += velocity * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), rotateSpeed * Time.deltaTime);
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
                handRay = new Ray(new Vector3(transform.position.x, 0.2f, transform.position.z), transform.forward);
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
                        table.Take(hand.HandUpObject);
                        hand.ClearHand();
                        return;
                    }

                    if(hitObj.TryGetComponent<HandNotAble.CrateController>(out var crate))
                    {
                        crate.Take(hand.HandUpObject);
                        hand.ClearHand();
                        return;
                    }
                }
                return;
            }

            hand.GetHandDown(null, new Vector3(hand.HandUpObject.transform.position.x, 0f, hand.HandUpObject.transform.position.z));
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

                    hand.GetHandUp(hitObj.transform.parent.gameObject);
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
                                case Utils.TableType.Packaging: (table as HandNotAble.PackagingTableController).Packaging(); break;
                            }
                        }
                    }
                }
            }
        }
    }
}