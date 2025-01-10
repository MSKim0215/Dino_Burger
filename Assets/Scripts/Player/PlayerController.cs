using UnityEngine;

namespace MSKim.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Hand hand;

        private float xAxis;
        private float zAxis;
        private float moveSpeed = 30f;
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
        }

        private void Pick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
                    hand.GetHandDown(hitObj.transform, new Vector3(0f, hand.HandUpObject.transform.localScale.y * hitObj.transform.localPosition.y, 0f));
                }
                return;
            }

            hand.GetHandDown(null, new Vector3(hand.HandUpObject.transform.position.x, hand.HandUpObject.transform.localScale.y / 2f, hand.HandUpObject.transform.position.z));
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
                        if(hitObj.transform.childCount >= 1)
                        {
                            hand.GetHandUp(hitObj.transform.GetChild(0).gameObject);
                        }
                        return;
                    }

                    hand.GetHandUp(hitObj);
                }
            }
        }
    }
}