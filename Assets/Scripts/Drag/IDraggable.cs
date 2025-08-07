using UnityEngine;

public interface IDraggable: IInteractable
{
    public void OnDragStart(Vector2 worldPos);
    public void OnDragUpdate(Vector2 worldPos);
    public void OnDragEnd();
}
