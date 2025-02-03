using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        transform.forward = Camera.main.transform.forward;
    }
}