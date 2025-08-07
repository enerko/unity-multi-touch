using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Collider2D))]
public class PinchableObject : MonoBehaviour, IPinchable
{
    private Vector3 _initialScale;
    private float _initialDistance;

    private const float _minScale = 0.5f;
    private const float _maxScale = 5.0f;

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

        // Calculate the new scale
        Vector3 newScale = _initialScale * scaleMultiplier;

        // Clamp each axis so object does not become too small or too big
        newScale.x = Mathf.Clamp(newScale.x, _minScale, _maxScale);
        newScale.y = Mathf.Clamp(newScale.y, _minScale, _maxScale);
        newScale.z = Mathf.Clamp(newScale.z, _minScale, _maxScale);

        // Log if scale is at max
        if (Mathf.Approximately(newScale.x, _maxScale) || Mathf.Approximately(newScale.y, _maxScale) ||
            Mathf.Approximately(newScale.z, _maxScale))
        {
            LogManager.Instance.LogInfo("Pinch", $"Attempted to pinch but {gameObject.name} already reached max scale!");
        }

        // Log if scale is at min
        if (Mathf.Approximately(newScale.x, _minScale) || Mathf.Approximately(newScale.y, _minScale) || 
            Mathf.Approximately(newScale.z, _minScale))
        {
            LogManager.Instance.LogInfo("Pinch", $"Attempted to pinch but {gameObject.name} already reached min scale!");
        }

        transform.localScale = newScale;
    }

    public void OnPinchEnd()
    {
        LogManager.Instance.LogInfo("Pinch", $"Pinch ended on {gameObject.name} with scale {transform.localScale}");
    }

}
