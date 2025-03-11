using System.Collections.Generic;
using UnityEngine;

public class SpawnPointInfo
{
    private List<Transform> points = new();

    public bool IsEmptyPoint() => points.Count <= 0;

    public void Clear() => points.Clear();

    public void AddPoint(Transform point) => points.Add(point);

    public int GetRandomIndex() => Random.Range(0, points.Count);

    public Transform GetPoint(int index) => points[index];

    public Transform GetRandomPoint() => points[Random.Range(0, points.Count)];
}
