using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RotatableObject : MonoBehaviour, IRotatable
{
    private Vector2 _initialVector;

    public void OnRotateStart(Vector2 pointA, Vector2 pointB)
    {
        LogManager.Instance.LogInfo("Rotate", $"Rotate started on {gameObject.name} with angle {transform.eulerAngles.z}");

        _initialVector = pointA - pointB;
    }

    public void OnRotateUpdate(Vector2 pointA, Vector2 pointB)
    {
        Vector2 currentVector = pointB - pointA;
        float deltaAngle = Vector2.SignedAngle(_initialVector, currentVector);

        transform.rotation *= Quaternion.Euler(0, 0, deltaAngle);
        _initialVector = currentVector;
    }

    public void OnRotateEnd()
    {
        LogManager.Instance.LogInfo("Rotate", $"Rotate ended on {gameObject.name} with angle {transform.eulerAngles.z}");
    }
}
