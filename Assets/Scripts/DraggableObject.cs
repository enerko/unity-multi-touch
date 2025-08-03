using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(Collider2D))]
public class DraggableObject : MonoBehaviour, IDraggable
{
    private Vector2 _offset;
    public void OnDragStart(Finger finger, Vector2 worldPos)
    {
        _offset = (Vector2)transform.position - worldPos;
        // Change color?
    }
    
    public void OnDragMove(Finger finger, Vector2 worldPos)
    {
        transform.position = worldPos + _offset;
    }
    public void OnDragEnd(Finger finger)
    {
        // Change color?
    }

}
