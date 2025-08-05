using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    
    private List<IPinchable> _pinchingObjects = new();
    private List<IRotatable> _rotatingObjects = new();

    private DragHandler _dragHandler;

    void Awake()
    {
        _dragHandler = gameObject.AddComponent<DragHandler>();
    }

    public void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();

        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerMove += HandleFingerMove;
        Touch.onFingerUp += HandleFingerUp;

        Touch.onFingerMove += _dragHandler.TryUpdateDrag;
        Touch.onFingerUp += _dragHandler.TryEndDrag;
    }

    public void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerMove -= HandleFingerMove;
        Touch.onFingerUp -= HandleFingerUp;

        // Drag
        Touch.onFingerMove -= _dragHandler.TryUpdateDrag;
        Touch.onFingerUp -= _dragHandler.TryEndDrag;

        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    public void HandleFingerDown(Finger finger)
    {
        Debug.Log($"Finger {finger.index} down at {finger.screenPosition}");

        // Start pinching
        if (Touch.activeFingers.Count >= 2)
        {
            TryStartPinchAndRotate();
        }

        _dragHandler.TryStartDrag(finger);
    }

    private void HandleFingerMove(Finger finger)
    {
        // Handle pinching
        if (Touch.activeFingers.Count >= 2)
        {
            TryUpdatePinchAndRotate();
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

        // Clear rotating
        if (_rotatingObjects.Count > 0)
        {
            _rotatingObjects[0].OnRotateEnd();
            _rotatingObjects.Clear();
        }
    }

    private void TryStartPinchAndRotate()
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

            IRotatable rotatable = pinchArea.GetComponent<IRotatable>();
            if (rotatable != null)
            {
                _rotatingObjects.Add(rotatable);
                rotatable.OnRotateStart(pointA, pointB);
            }
        }
    }

    private void TryUpdatePinchAndRotate()
    {
        Vector2 pointA = Camera.main.ScreenToWorldPoint(Touch.activeFingers[0].screenPosition);
        Vector2 pointB = Camera.main.ScreenToWorldPoint(Touch.activeFingers[1].screenPosition);

        _pinchingObjects[0].OnPinchScale(pointA, pointB);
        _rotatingObjects[0].OnRotateChange(pointA, pointB);
    }

    
}
