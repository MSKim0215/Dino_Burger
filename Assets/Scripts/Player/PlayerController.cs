using UnityEngine;

namespace MSKim.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float xAxis;
        private float zAxis;
        private float moveSpeed = 30f;
        private float rotateSpeed = 10f;

        private RaycastHit hit;
        private Ray ray;
        private float raycastDistance = 3f;

        [Header("My Hand")]
        [SerializeField] private Transform handTransform;
        [SerializeField] private GameObject handUpObject;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            xAxis = Input.GetAxis("Horizontal");
            zAxis = Input.GetAxis("Vertical");

            var velocity = new Vector3(xAxis, 0f, zAxis);

            if (!(xAxis == 0f && zAxis == 0f))
            {
                transform.position += velocity * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), rotateSpeed * Time.deltaTime);
            }
        }

        private void Update()
        {
            Pick();
        }

        private void Pick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ray = new Ray(new Vector3(transform.position.x, 0.2f, transform.position.z), transform.forward);
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * raycastDistance, Color.red);

                if (handUpObject != null)
                {
                    PickDown();
                    return;
                }

                PickUp();
            }
        }

        private void PickDown()
        {
            int layerMask = 1 << LayerMask.NameToLayer("HandNotAble");
            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
                var hitObj = hit.collider.gameObject;
                if(hitObj != null)
                {
                    handUpObject.transform.SetParent(hitObj.transform);
                    handUpObject.transform.localPosition = new Vector3(0f, handUpObject.transform.localScale.y - hitObj.transform.position.y, 0f);
                    handUpObject = null;
                }
                return;
            }

            handUpObject.transform.SetParent(null);
            handUpObject.transform.localPosition = new Vector3(handUpObject.transform.localPosition.x, handUpObject.transform.localScale.y / 2f, handUpObject.transform.localPosition.z);
            handUpObject = null;
        }

        private void PickUp()
        {
            int layerMask = (1 << LayerMask.NameToLayer("HandAble")) + (1 << LayerMask.NameToLayer("HandNotAble"));
            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
                var hitObj = hit.collider.gameObject;
                if (hitObj != null)
                {
                    if(hitObj.layer == LayerMask.NameToLayer("HandAble"))
                    {
                        handUpObject = hitObj;
                        handUpObject.transform.SetParent(handTransform);
                        handUpObject.transform.localPosition = Vector3.zero;
                    }
                    else if(hitObj.layer == LayerMask.NameToLayer("HandNotAble"))
                    {
                        if(hitObj.transform.GetChild(0) != null)
                        {
                            handUpObject = hitObj.transform.GetChild(0).gameObject;
                            handUpObject.transform.SetParent(handTransform);
                            handUpObject.transform.localPosition = Vector3.zero;
                        }
                    }
                }
            }
        }
    }
}