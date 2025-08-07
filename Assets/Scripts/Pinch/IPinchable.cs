using UnityEngine;

public interface IPinchable
{
    public void OnPinchStart(Vector2 pointA, Vector2 pointB);
    public void OnPinchUpdate(Vector2 pointA, Vector2 pointB);
    public void OnPinchEnd();
}
