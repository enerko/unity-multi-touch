using UnityEngine;

public class PinchHandler : MonoBehaviour
{
    private IPinchable _pinchingObject;

    public void TryStartPinch(Vector2 pointA, Vector2 pointB)
    {
        // Ignore if there is an ongoing process
        if (_pinchingObject != null)
            return;

        Collider2D pinchArea = Physics2D.OverlapArea(pointA, pointB);

        if (pinchArea != null)
        {
            if (pinchArea.TryGetComponent<IPinchable>(out var pinchable))
            {
                // Verify that there are no other objects currently interacted with
                if (InteractionManager.TryStartInteraction(pinchable))
                {
                    _pinchingObject = pinchable;
                    pinchable.OnPinchStart(pointA, pointB);
                }
            }
            
        }
        else
        {
            LogManager.Instance.LogInfo("Pinch", $"Pinch attempted but no pinchable component found at area between {pointA} and {pointB}");
        }
    }

    public void TryUpdatePinch(Vector2 pointA, Vector2 pointB)
    {
        _pinchingObject?.OnPinchUpdate(pointA, pointB);
    }

    public void TryEndPinch()
    {
        InteractionManager.EndInteraction(_pinchingObject);
        _pinchingObject?.OnPinchEnd();
        _pinchingObject = null;
    }
}
