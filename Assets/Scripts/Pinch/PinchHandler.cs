using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PinchHandler : MonoBehaviour
{
    private IPinchable _pinchingObject;

    public void TryStartPinch(Vector2 pointA, Vector2 pointB)
    {
        Collider2D pinchArea = Physics2D.OverlapArea(pointA, pointB);

        if (pinchArea != null)
        {
            if (pinchArea.TryGetComponent<IPinchable>(out var pinchable))
            {
                _pinchingObject = pinchable;
                pinchable.OnPinchStart(pointA, pointB);
            }
        }
    }

    public void TryUpdatePinch(Vector2 pointA, Vector2 pointB)
    {
        _pinchingObject.OnPinchUpdate(pointA, pointB);
    }

    public void TryEndPinch()
    {
        _pinchingObject.OnPinchEnd();
    }
}
