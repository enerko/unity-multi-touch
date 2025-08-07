using UnityEngine;

public class PinchableObject : MonoBehaviour, IPinchable
{
    private Vector3 _initialScale;
    private float _initialDistance;

    public void OnPinchStart(Vector2 pointA, Vector2 pointB)
    {
        Debug.Log($"Pinch started on {gameObject.name}");
        _initialScale = transform.localScale; 
        _initialDistance = Vector2.Distance(pointA, pointB);
    }

    public void OnPinchUpdate(Vector2 pointA, Vector2 pointB)
    {
        float distance = Vector2.Distance(pointA, pointB);

        float scaleMultiplier = distance / _initialDistance;
        transform.localScale = _initialScale * scaleMultiplier;
    }

    public void OnPinchEnd()
    {
        Debug.Log($"Pinch ended on {gameObject.name}");
    }

}
