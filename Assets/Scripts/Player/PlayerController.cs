using UnityEngine;

namespace MSKim.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float xAxis;
        private float zAxis;
        private float moveSpeed = 30f;
        private float rotateSpeed = 10f;

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
    }
}