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
            Pick();
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

        private void Pick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * raycastDistance, Color.red);

                ray = new Ray(transform.position, transform.forward);

                if (Physics.Raycast(ray, out hit, raycastDistance))
                {
                    var hitObj = hit.collider.gameObject;
                    if(hitObj != null)
                    {
                        handUpObject = hitObj;
                        handUpObject.transform.SetParent(handTransform);
                        handUpObject.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
    }
}