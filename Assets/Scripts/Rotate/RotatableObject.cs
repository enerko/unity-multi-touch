using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RotatableObject : MonoBehaviour, IRotatable
{
    private Vector2 _initialVector;

    public void OnRotateStart(Vector2 pointA, Vector2 pointB)
    {
        LogManager.Instance.LogInfo("Rotate", $"Rotate started on {gameObject.name} with angle {transform.eulerAngles.z}");

        // Calculate and store the initial vector between the two touch points
        _initialVector = pointB - pointA;
    }

    public void OnRotateUpdate(Vector2 pointA, Vector2 pointB)
    {
        Vector2 currentVector = pointB - pointA;

        // Determine the angle difference between the initial vector and the current one
        float deltaAngle = Vector2.SignedAngle(_initialVector, currentVector);

        // Apply the rotation to the GameObject around the Z-axis
        transform.rotation *= Quaternion.Euler(0, 0, deltaAngle);
        _initialVector = currentVector;
    }

    public void OnRotateEnd()
    {
        LogManager.Instance.LogInfo("Rotate", $"Rotate ended on {gameObject.name} with angle {transform.eulerAngles.z}");
    }
}
