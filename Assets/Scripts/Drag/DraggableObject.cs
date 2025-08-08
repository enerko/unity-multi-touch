using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableObject : MonoBehaviour, IDraggable
{
    private Vector2 _offset;
    public void OnDragStart(Vector2 worldPos)
    {
        LogManager.Instance.LogInfo("Drag", $"Drag started on {gameObject.name} at position {worldPos}");
        _offset = (Vector2)transform.position - worldPos;
    }
    
    public void OnDragUpdate(Vector2 worldPos)
    {
        transform.position = worldPos + _offset;
    }

    public void OnDragEnd()
    {
        LogManager.Instance.LogInfo("Drag", $"Drag ended on {gameObject.name} at {(Vector2)transform.position}");
    }

}
