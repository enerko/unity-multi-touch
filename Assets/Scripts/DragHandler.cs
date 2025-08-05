using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DragHandler : MonoBehaviour
{
    private IDraggable _draggingObject;

    public void TryStartDrag(Finger finger)
    {
        // TODO?: If dragging is already taking place, should not override until released

        // Convert finger position to world position
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            // Check if the finger position overlaps with a draggable object
            IDraggable draggable = hit.GetComponent<IDraggable>();

            if (draggable != null)
            {
                _draggingObject = draggable;
                draggable.OnDragStart(finger, worldPos);
            }
        }
    }

    public void TryUpdateDrag(Finger finger)
    {
        // TODO: Check this is the finger dragging the object
        if (_draggingObject != null)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
            _draggingObject.OnDragMove(finger, worldPos);
        }
    }

    public void TryEndDrag(Finger finger)
    {
        // TODO: Check this is the finger dragging the object
        // Clear dragging
        if (_draggingObject != null)
        {
            _draggingObject.OnDragEnd(finger);
            _draggingObject = null;
        }
    }
}
