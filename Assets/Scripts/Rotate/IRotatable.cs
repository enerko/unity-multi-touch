using UnityEngine;

public interface IRotatable: IInteractable
{
    public void OnRotateStart(Vector2 pointA, Vector2 pointB);
    public void OnRotateUpdate(Vector2 pointA, Vector2 pointB);
    public void OnRotateEnd();
}
