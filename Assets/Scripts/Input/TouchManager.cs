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


        // Visualize 
        StartVisualizeTouch(finger);

        // Ignore if there are already 2 fingers down

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
        // Ignore if it is the 3rd finger or above

        UpdateVisualizeTouch(finger);

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
        LogManager.Instance.LogInfo("Input", $"Finger {finger.index} released");

        _dragHandler.TryEndDrag();
        _pinchHandler.TryEndPinch();
        _rotateHandler.TryEndRotate();

        EndVisualizeTouch(finger);
    }

    private void StartVisualizeTouch(Finger finger)
    {
        if (!DebugParameters.IsVisualizeTouch) return;

        Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        GameObject clone = Instantiate(_touchCirclePrefab, fingerPosition, Quaternion.identity);

        _touchCircleClones[finger.index] = clone;
    }

    private void UpdateVisualizeTouch(Finger finger)
    {
        if (!DebugParameters.IsVisualizeTouch) return;

        Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);

        _touchCircleClones[finger.index].transform.position = fingerPosition;
    }

    private void EndVisualizeTouch(Finger finger)
    {
        if (!DebugParameters.IsVisualizeTouch) return;

        Destroy(_touchCircleClones[finger.index]);
        _touchCircleClones[finger.index] = null;
    }
}
