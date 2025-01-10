using System;
using UnityEngine;

[Serializable]
public class Hand
{
    [Header("My Hand")]
    [SerializeField] private Transform handTransform;
    [SerializeField] private GameObject handUpObject;

    public GameObject HandUpObject => handUpObject;

    public void GetHandDown(Transform downTransform, Vector3 downPosition)
    {
        handUpObject.transform.SetParent(downTransform);
        handUpObject.transform.localPosition = downPosition;
        handUpObject = null;
    }

    public void GetHandUp(GameObject target)
    {
        handUpObject = target;
        handUpObject.transform.SetParent(handTransform);
        handUpObject.transform.localPosition = Vector3.zero;
    }
}
