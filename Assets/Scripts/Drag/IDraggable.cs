using UnityEngine;

public interface IDraggable
{
    public void OnDragStart(Vector2 worldPos);
    public void OnDragUpdate(Vector2 worldPos);
    public void OnDragEnd();
}
