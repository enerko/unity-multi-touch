using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    public GameObject circle;
    public GameObject[] circleClones;

    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
    }

    private void FingerDown(Finger finger)
    {
        Vector2 fingerPosition = mainCamera.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        GameObject clone = Instantiate(circle, fingerPosition, Quaternion.identity);

        circleClones[finger.index] = clone.gameObject;
    }

    private void FingerUp(Finger finger)
    {
        Destroy(circleClones[finger.index]);
        circleClones[finger.index] = null;
    }
}
