using UnityEngine;

public interface IRotatable
{
    public void OnRotateStart(Vector2 pointA, Vector2 pointB);
    public void OnRotateUpdate(Vector2 pointA, Vector2 pointB);
    public void OnRotateEnd();
}
