using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    private Dictionary<Finger, IDraggable> _draggingObjects = new();
    private List<IPinchable> _pinchingObjects = new();

    public void OnEnable()
    {
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
        Debug.Log($"Finger {finger.index} down at {finger.screenPosition}");

        // Start pinching
        if (Touch.activeFingers.Count == 2)
        {
            Vector2 pointA = Camera.main.ScreenToWorldPoint(Touch.activeFingers[0].screenPosition);
            Vector2 pointB = Camera.main.ScreenToWorldPoint(Touch.activeFingers[1].screenPosition);

            Collider2D pinchArea = Physics2D.OverlapArea(pointA, pointB);

            if (pinchArea != null)
            {
                IPinchable pinchable = pinchArea.GetComponent<IPinchable>();
                if (pinchable != null)
                {
                    _pinchingObjects.Add(pinchable);
                    pinchable.OnPinchStart(pointA, pointB);
                }
            }
            return;
        }

        // Start dragging

        // Convert finger position to world position
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            // Check if the finger position overlaps with a draggable object
            IDraggable draggable = hit.GetComponent<IDraggable>();

            if (draggable != null)
            {
                _draggingObjects[finger] = draggable;
                draggable.OnDragStart(finger, worldPos);
            }
        }
    }

    private void HandleFingerMove(Finger finger)
    {
        // Handle pinching
        if (Touch.activeFingers.Count == 2)
        {
            Vector2 pointA = Camera.main.ScreenToWorldPoint(Touch.activeFingers[0].screenPosition);
            Vector2 pointB = Camera.main.ScreenToWorldPoint(Touch.activeFingers[1].screenPosition);

            _pinchingObjects[0].OnPinchScale(pointA, pointB);
            return;
        }

        // Handle dragging
        if (_draggingObjects.TryGetValue(finger, out IDraggable draggable))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
            draggable.OnDragMove(finger, worldPos);
        }
    }

    public void HandleFingerUp(Finger finger)
    {
        // Clear pinching
        if (_pinchingObjects.Count > 0)
        {
            _pinchingObjects[0].OnPinchEnd();
            _pinchingObjects.Clear();
        }

        // Clear dragging
        if (_draggingObjects.TryGetValue(finger, out IDraggable draggable))
        {
            draggable.OnDragEnd(finger);
            _draggingObjects.Remove(finger);
        }
    }

}
