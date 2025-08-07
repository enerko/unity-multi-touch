using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableObject : MonoBehaviour, IDraggable
{
    private Vector2 _offset;
    public void OnDragStart(Vector2 worldPos)
    {
        LogManager.Instance.LogInfo("Drag", $"Drag started on {gameObject.name}");
        _offset = (Vector2)transform.position - worldPos;
        // Change color?
    }
    
    public void OnDragUpdate(Vector2 worldPos)
    {
        transform.position = worldPos + _offset;
    }
    public void OnDragEnd()
    {
        // Change color?
        LogManager.Instance.LogInfo("Drag", $"Drag ended on {gameObject.name}");
    }

}
