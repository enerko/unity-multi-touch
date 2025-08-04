using UnityEngine;

public class PinchableObject : MonoBehaviour, IPinchable
{
    private float _initialScale = 1f;
    private float _initialDistance;

    public void OnPinchStart(Vector2 pointA, Vector2 pointB)
    {
        LogManager.Instance.Log($"Pinch started on {gameObject.name}");
        _initialDistance = Vector2.Distance(pointA, pointB);
    }

    public void OnPinchScale(Vector2 pointA, Vector2 pointB)
    {
        float distance = Vector2.Distance(pointA, pointB);

        float newScale = (distance / _initialDistance) * _initialScale;
        transform.localScale = Vector3.one * newScale;
    }

    public void OnPinchEnd()
    {
        LogManager.Instance.Log($"Pinch ended on {gameObject.name}");
    }

}
