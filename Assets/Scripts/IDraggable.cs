using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public interface IDraggable
{
    public void OnDragStart(Finger finger, Vector2 worldPos);
    public void OnDragMove(Finger finger, Vector2 worldPos);
    public void OnDragEnd(Finger finger);
}
