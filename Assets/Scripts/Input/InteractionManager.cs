using UnityEngine;

public class InteractionManager
{
    private static GameObject _currentInteraction;

    public static bool TryStartInteraction(IInteractable target)
    {
        var go = (target as MonoBehaviour).gameObject;
        if (_currentInteraction != null && _currentInteraction != go)
        {
            LogManager.Instance.LogInfo("Interact", $"Attempted to interact with {go} but another interaction with {_currentInteraction} is already taking place");
            return false;
        }
            
        _currentInteraction = go;
        return true;
    }

    public static void EndInteraction(IInteractable target)
    {
        var go = (target as MonoBehaviour).gameObject;
        if (_currentInteraction == go)
        {
            _currentInteraction = null;
        }
    }
}