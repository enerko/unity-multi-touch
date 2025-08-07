using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DragHandler : MonoBehaviour
{
    private IDraggable _draggingObject;

    public void TryStartDrag(Vector2 point)
    {
        // TODO?: If dragging is already taking place, should not override until released

        
        Collider2D hit = Physics2D.OverlapPoint(point);

        if (hit != null)
        {
            // Check if the finger position overlaps with a draggable object
            
            if (hit.TryGetComponent<IDraggable>(out var draggable))
            {
                Debug.Log("Starting drag");
                _draggingObject = draggable;
                draggable.OnDragStart(point);
            }
        }
    }

    public void TryUpdateDrag(Vector2 point)
    {
        // TODO: Check this is the finger dragging the object
        if (_draggingObject != null)
        {
            _draggingObject.OnDragUpdate(point);
        }
    }

    public void TryEndDrag()
    {
        // TODO: Check this is the finger dragging the object
        // Clear dragging
        if (_draggingObject != null)
        {
            _draggingObject.OnDragEnd();
            _draggingObject = null;
        }
    }
}
