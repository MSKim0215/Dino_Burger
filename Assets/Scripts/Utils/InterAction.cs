using System;
using UnityEngine;

public interface IBaseInterAction
{
    public void Take(GameObject takeObject);
    public GameObject Give();
}

[Serializable]
public abstract class InterActionMonoBehaviour : MonoBehaviour, IBaseInterAction
{
    [Header("Hightlight Settings")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material[] baseMats;
    [SerializeField] private Material[] highMats;

    private bool isActiveHighlight = false;

    public bool IsActiveHightlight
    {
        get => isActiveHighlight;
        set
        {
            if (isActiveHighlight == value) return;

            isActiveHighlight = value;
            SetActiveHighlight();
        }
    }

    private void SetActiveHighlight()
    {
        if (isActiveHighlight)
        {
            for(int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = highMats;
            }
        }
        else
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = baseMats;
            }
        }
    }

    public abstract void Take(GameObject takeObject);
    public abstract GameObject Give();
}

public interface IToolInterAction
{
    public void TakeTool(GameObject takeObject);
    public GameObject GiveTool();
}