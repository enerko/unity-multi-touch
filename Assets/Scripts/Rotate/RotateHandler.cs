using UnityEngine;

public class RotateHandler : MonoBehaviour
{
    private IRotatable _rotatableObject;

    public void TryStartRotate(Vector2 pointA, Vector2 pointB)
    {
        // Ignore if there is an ongoing process
        if (_rotatableObject != null)
            return;

        Collider2D rotateArea = Physics2D.OverlapArea(pointA, pointB);

        if (rotateArea != null)
        {
            if (rotateArea.TryGetComponent<IRotatable>(out var rotatable))
            {
                // Verify that there are no other objects currently interacted with
                if (InteractionManager.TryStartInteraction(rotatable))
                {
                    _rotatableObject = rotatable;
                    rotatable.OnRotateStart(pointA, pointB);
                }
            }
        }
        else
        {
            LogManager.Instance.LogInfo("Rotate", $"Rotate attempted but no rotatable component found at area between {pointA} and {pointB}");
        }
    }

    public void TryUpdateRotate(Vector2 pointA, Vector2 pointB)
    {
        _rotatableObject?.OnRotateUpdate(pointA, pointB);
    }

    public void TryEndRotate()
    {
        InteractionManager.EndInteraction(_rotatableObject);
        _rotatableObject?.OnRotateEnd();
        _rotatableObject = null;
    }
}
