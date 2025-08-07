using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    private DragHandler _dragHandler;
    private PinchHandler _pinchHandler;
    private RotateHandler _rotateHandler;

    void Awake()
    {
        _dragHandler = gameObject.AddComponent<DragHandler>();
        _pinchHandler = gameObject.AddComponent<PinchHandler>();
        _rotateHandler = gameObject.AddComponent<RotateHandler>();
    }

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



        // Visualize 


        Vector2 pointA = Camera.main.ScreenToWorldPoint(Touch.activeFingers[0].screenPosition);

        // Start pinching
        if (Touch.activeFingers.Count >= 2)
        {
            Vector2 pointB = Camera.main.ScreenToWorldPoint(Touch.activeFingers[1].screenPosition);

            _pinchHandler.TryStartPinch(pointA, pointB);
            _rotateHandler.TryStartRotate(pointA, pointB);
        }

        _dragHandler.TryStartDrag(pointA);
    }

    private void HandleFingerMove(Finger finger)
    {
        Vector2 pointA = Camera.main.ScreenToWorldPoint(Touch.activeFingers[0].screenPosition);

        // Handle pinching
        if (Touch.activeFingers.Count >= 2)
        {
            Vector2 pointB = Camera.main.ScreenToWorldPoint(Touch.activeFingers[1].screenPosition);
            _pinchHandler.TryUpdatePinch(pointA, pointB);
            _rotateHandler.TryUpdateRotate(pointA, pointB);
        }

        _dragHandler.TryUpdateDrag(pointA);
    }

    public void HandleFingerUp(Finger finger)
    {
        _dragHandler.TryEndDrag();
        _pinchHandler.TryEndPinch();
        _rotateHandler.TryEndRotate();
    }
}
