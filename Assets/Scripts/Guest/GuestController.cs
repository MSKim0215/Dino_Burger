using MSKim.Manager;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : CharacterController
    {
        [Header("Waypoint Settings")]
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private float checkDistance = 0.01f;

        private void Start()
        {
            moveSpeed = Random.Range(1f, 2.5f);
            rotateSpeed = 5f;   
        }

        private void FixedUpdate()
        {
            if (currentPointIndex >= 2) return;

            base.Move();
            CheckDistance();
        }

        public override void MovePosition()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, WaypointManager.Instance.GetCurrentWayPoint(currentPointIndex), moveSpeed * Time.deltaTime);
        }

        public override void MoveRotation()
        {
            var direction = WaypointManager.Instance.GetCurrentWayPoint(currentPointIndex) - transform.position;
            direction.Normalize();

            var rotationFixedY = CalculateRotationY(transform.rotation, direction);
            var rotationY = rotationFixedY;

            if (rotationY * rotationFixedY > 0f)
            {
                Quaternion rotationAmount = Quaternion.Euler(0f, rotateSpeed * rotationFixedY * Time.deltaTime, 0f);
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

        private void CheckDistance()
        {
            if (Vector3.Distance(WaypointManager.Instance.GetCurrentWayPoint(currentPointIndex), transform.position) <= checkDistance)
            {
                currentPointIndex++;
            }
        }
    }
}