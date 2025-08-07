using UnityEngine;

public class RotateHandler : MonoBehaviour
{
    private IRotatable _rotatableObject;

    public void TryStartRotate(Vector2 pointA, Vector2 pointB)
    {
        Collider2D rotateArea = Physics2D.OverlapArea(pointA, pointB);

        if (rotateArea != null)
        {
            if (rotateArea.TryGetComponent<IRotatable>(out var rotatable))
            {
                _rotatableObject = rotatable;
                _rotatableObject.OnRotateStart(pointA, pointB);
            }
        }
        else
        {
            LogManager.Instance.LogWarning("Rotate", $"Rotate attempted but no rotatable component found at area between {pointA} and {pointB}");
        }
    }

    public void TryUpdateRotate(Vector2 pointA, Vector2 pointB)
    {
        _rotatableObject?.OnRotateUpdate(pointA, pointB);
    }

    public void TryEndRotate()
    {
        _rotatableObject?.OnRotateEnd();
    }
}
