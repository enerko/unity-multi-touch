using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    private Dictionary<Finger, IDraggable> draggingObjects = new();

    public void OnEnable()
    {
        Debug.Log("enabled");
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();

        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerMove += HandleFingerMove;
        Touch.onFingerUp += HandleFingerUp;
    }

    public void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerMove -= HandleFingerMove;
        Touch.onFingerUp -= HandleFingerUp;

        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    public void HandleFingerDown(Finger finger)
    {
        // Convert finger position to world position
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            // Check if the finger position overlaps with a draggable object
            IDraggable draggable = hit.GetComponent<IDraggable>();

            if (draggable != null)
            {
                draggingObjects[finger] = draggable;
                draggable.OnDragStart(finger, worldPos);
            }
        }
    }

    private void HandleFingerMove(Finger finger)
    {
        if (draggingObjects.TryGetValue(finger, out IDraggable draggable))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
            draggable.OnDragMove(finger, worldPos);
        }
    }

    public void HandleFingerUp(Finger finger)
    {
        if (draggingObjects.TryGetValue(finger, out IDraggable draggable))
        {
            draggable.OnDragEnd(finger);
            draggingObjects.Remove(finger);
        }
    }

}
