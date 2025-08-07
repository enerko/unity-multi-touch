using UnityEngine;

public class RotatableObject : MonoBehaviour, IRotatable
{
    private Vector2 _initialVector;

    public void OnRotateStart(Vector2 pointA, Vector2 pointB)
    {
        Debug.Log($"Rotate started on {gameObject.name}");

        _initialVector = pointA - pointB;
    }

    public void OnRotateUpdate(Vector2 pointA, Vector2 pointB)
    {
        Vector2 currentVector = pointB - pointA;
        float deltaAngle = Vector2.SignedAngle(_initialVector, currentVector);

        Debug.Log($"Rotating {gameObject.name} by {deltaAngle:F2} degrees");

        transform.rotation *= Quaternion.Euler(0, 0, deltaAngle);
        _initialVector = currentVector;
    }

    public void OnRotateEnd()
    {
        Debug.Log($"Rotate ended on {gameObject.name}");
    }

    private float GetAngle(Vector2 pointA, Vector2 pointB)
    {
        return Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x);
    }
}
