using MSKim.Manager;
using UnityEngine;

namespace MSKim.NonPlayer
{
    public class GuestController : CharacterController
    {
        [Header("Waypoint Settings")]
        [SerializeField] private int currentPointIndex = 0;
        [SerializeField] private float checkDistance = 0.5f;

        private float firstSpawnPointZ;
        private bool isOutside = false;
        private Vector3 outsidePoint;

        private void Start()
        {
            moveSpeed = 1.5f;
            rotateSpeed = 5f;
            checkDistance = 0.8f;

            firstSpawnPointZ = transform.position.z;
        }

        private void FixedUpdate()
        {
            if (currentPointIndex >= 2) return;

            base.Move();
            CheckDistance();
        }

        public override void MovePosition()
        {
            var targetPoint = WaypointManager.Instance.GetCurrentWayPoint(currentPointIndex);

            if(!isOutside && currentPointIndex == 1)
            {
                isOutside = true;
                outsidePoint = Random.Range(0, 2) == 0 ? WaypointManager.Instance.GetOutsideRightPoint(currentPointIndex)
                    : WaypointManager.Instance.GetOutsideLeftPoint(currentPointIndex);
            }

            transform.position =
                Vector3.MoveTowards(transform.position, currentPointIndex == 1 ? outsidePoint : targetPoint, moveSpeed * Time.deltaTime);
            if(currentPointIndex == 0)
            {
                transform.position = new(transform.position.x, transform.position.y, firstSpawnPointZ);
            }
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
            if(currentPointIndex == 0)
            {
                var target = WaypointManager.Instance.GetCurrentWayPoint(currentPointIndex);
                float currDistance = Vector3.Distance(new(target.x, 0f, 0f), new(transform.position.x, 0f, 0f));
                if (currDistance <= checkDistance)
                {
                    currentPointIndex++;
                }
                return;
            }

            if (Vector3.Distance(WaypointManager.Instance.GetCurrentWayPoint(currentPointIndex), transform.position) <= checkDistance)
            {
                currentPointIndex++;
            }
        }
    }
}