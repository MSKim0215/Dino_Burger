using System;
using UnityEngine;

[Serializable]
public class Hand
{
    [SerializeField] private Transform handRoot;
    [SerializeField] private GameObject handUpObject;

    public GameObject HandUpObject => handUpObject;

    public Utils.CrateType HandUpObjectType
    {
        get
        {
            var ingredient = GetHandUpComponent<MSKim.HandAble.IngredientController>();
            return ingredient != null ? ingredient.IngredientType : Utils.CrateType.None;
        }
    }

    public Utils.IngredientState HandUpObjectState
    {
        get
        {
            var ingredient = GetHandUpComponent<MSKim.HandAble.IngredientController>();
            return ingredient != null ? ingredient.IngredientState : Utils.IngredientState.None;
        }
    }

    public bool IsHandUpObjectFood() => handUpObject.CompareTag("Food");

    public T GetHandUpComponent<T>()
    {
        if (handUpObject == null) return default;
        return handUpObject.TryGetComponent(out T component) ? component : default;
    }

    public void ClearHand() => handUpObject = null;

    public void GetHandDown(Transform downTransform, Vector3 downPosition)
    {
        handUpObject.transform.SetParent(downTransform);
        handUpObject.transform.localPosition = downPosition;
        ClearHand();
    }

    public void GetHandUp(GameObject target)
    {
        SetHandUpObject(target);
        GetHandUp();
    }

    public void SetHandUpObject(GameObject target)
    {
        if (target == null) return;

        handUpObject = target;
    }

    public void GetHandUp()
    {
        if (handUpObject == null) return;

        handUpObject.transform.SetParent(handRoot);
        handUpObject.transform.localPosition = Vector3.zero;
    }
}
