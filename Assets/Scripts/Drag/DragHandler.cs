using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private IDraggable _draggingObject;

    public void TryStartDrag(Vector2 point)
    {
        // Ignore if there is an ongoing process
        if (_draggingObject != null)
            return;

        Collider2D hit = Physics2D.OverlapPoint(point);

        if (hit != null)
        {
            // Check if the finger position overlaps with a draggable object
            if (hit.TryGetComponent<IDraggable>(out var draggable))
            {
                // Verify that there are no other objects currently interacted with
                if (InteractionManager.TryStartInteraction(draggable as IInteractable))
                {
                    _draggingObject = draggable;
                    _draggingObject.OnDragStart(point);
                }
            }
        }
        else
        {
            LogManager.Instance.LogInfo("Drag", $"Drag attempted but no draggable component found at {point}");
        }
    }

    public void TryUpdateDrag(Vector2 point)
    {
        _draggingObject?.OnDragUpdate(point);
    }

    public void TryEndDrag()
    {
        if (_draggingObject != null)
        {
            InteractionManager.EndInteraction(_draggingObject as IInteractable);
            _draggingObject.OnDragEnd();
        }
        
        _draggingObject = null;
    }
}
