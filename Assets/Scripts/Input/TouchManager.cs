using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    private DragHandler _dragHandler;
    private PinchHandler _pinchHandler;
    private RotateHandler _rotateHandler;

    [SerializeField] private GameObject _touchCirclePrefab;
    private GameObject[] _touchCircleClones = new GameObject[10]; // Assuming only 10 fingers are used

    private int _activeTouchCounter = 0;

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
        LogManager.Instance.LogInfo("Input", $"Finger {finger.index} down");

        StartVisualizeTouch(finger);
        _activeTouchCounter++;

        // Start pinch/rotate only if this finger is one of the first two active fingers
        if (_activeTouchCounter >= 2)
        {
            var firstFinger = Touch.activeFingers[0];
            var secondFinger = Touch.activeFingers[1];

            if (finger == Touch.activeFingers[0] || finger == Touch.activeFingers[1])
            {
                Vector2 pointA = Camera.main.ScreenToWorldPoint(firstFinger.screenPosition);
                Vector2 pointB = Camera.main.ScreenToWorldPoint(secondFinger.screenPosition);

                _pinchHandler.TryStartPinch(pointA, pointB);
                _rotateHandler.TryStartRotate(pointA, pointB);
            }

            return; // skip drag for these fingers since pinch/rotate are starting
        }

        // Otherwise, since we know this is the first finger, try drag
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        _dragHandler.TryStartDrag(worldPos);
    }

    private void HandleFingerMove(Finger finger)
    {
        UpdateVisualizeTouch(finger);

        var firstFinger = Touch.activeFingers[0];

        // Start pinch/rotate only if this finger is one of the first two active fingers
        if (_activeTouchCounter >= 2)
        {
            var secondFinger = Touch.activeFingers[1];

            if (finger == firstFinger || finger == secondFinger)
            {
                Vector2 pointA = Camera.main.ScreenToWorldPoint(firstFinger.screenPosition);
                Vector2 pointB = Camera.main.ScreenToWorldPoint(secondFinger.screenPosition);

                _pinchHandler.TryUpdatePinch(pointA, pointB);
                _rotateHandler.TryUpdateRotate(pointA, pointB);
            }
        }

        // Only update drag if this is the first finger
        if (finger == firstFinger)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
            _dragHandler.TryUpdateDrag(worldPos);
        }
        
    }

    public void HandleFingerUp(Finger finger)
    {
        LogManager.Instance.LogInfo("Input", $"Finger {finger.index} released");
        _activeTouchCounter--;

        if (_activeTouchCounter < 2)
        {
            _pinchHandler.TryEndPinch();
            _rotateHandler.TryEndRotate();
        }

        if (_activeTouchCounter == 0)
            _dragHandler.TryEndDrag();

        EndVisualizeTouch(finger);
    }

    private void StartVisualizeTouch(Finger finger)
    {
        if (!DebugParameters.IsVisualizeTouch) 
            return;

        Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        GameObject clone = Instantiate(_touchCirclePrefab, fingerPosition, Quaternion.identity);

        _touchCircleClones[finger.index] = clone;
    }

    private void UpdateVisualizeTouch(Finger finger)
    {
        if (!DebugParameters.IsVisualizeTouch) 
            return;

        Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);

        _touchCircleClones[finger.index].transform.position = fingerPosition;
    }

    private void EndVisualizeTouch(Finger finger)
    {
        if (!DebugParameters.IsVisualizeTouch) 
            return;

        Destroy(_touchCircleClones[finger.index]);
        _touchCircleClones[finger.index] = null;
    }
}
