using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool isUpdate = false;

    private void Start()
    {
        RefreshForward();
    }

    private void Update()
    {
        if (!isUpdate) return;

        RefreshForward();
    }

    private void RefreshForward()
    {
        transform.forward = Camera.main.transform.forward;
    }
}