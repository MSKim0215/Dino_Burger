using UnityEngine;

public interface IBaseInterAction
{
    public void Take(GameObject takeObject);
    public GameObject Give();
}

public interface IToolInterAction
{
    public void TakeTool(GameObject takeObject);
    public GameObject GiveTool();
}