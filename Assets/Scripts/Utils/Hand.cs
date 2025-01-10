using System;
using UnityEngine;

[Serializable]
public class Hand
{
    [SerializeField] private Transform handRoot;
    [SerializeField] private GameObject handUpObject;

    public GameObject HandUpObject => handUpObject;

    public void ClearHand() => handUpObject = null;

    public void GetHandDown(Transform downTransform, Vector3 downPosition)
    {
        handUpObject.transform.SetParent(downTransform);
        handUpObject.transform.localPosition = downPosition;
        ClearHand();
    }

    public void GetHandUp(GameObject target)
    {
        handUpObject = target;
        handUpObject.transform.SetParent(handRoot);
        handUpObject.transform.localPosition = Vector3.zero;
    }
}
