using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PinchableObject : MonoBehaviour, IPinchable
{
    private Vector3 _initialScale;
    private float _initialDistance;

    public void OnPinchStart(Vector2 pointA, Vector2 pointB)
    {
        _initialScale = transform.localScale;

        LogManager.Instance.LogInfo("Pinch", $"Pinch started on {gameObject.name} with scale {_initialScale}");

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
        LogManager.Instance.LogInfo("Pinch", $"Pinch ended on {gameObject.name} with scale {transform.localScale}");
    }

}
