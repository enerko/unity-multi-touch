public class InteractionManager
{
    private static IInteractable _currentInteraction;

    public static bool TryStartInteraction(IInteractable target)
    {
        if (_currentInteraction != null && _currentInteraction != target)
        {
            LogManager.Instance.LogInfo("Interact", $"Attempted to interact with {target} but another interaction with {_currentInteraction} is already taking place");
            return false;
        }
            
        _currentInteraction = target;
        return true;
    }

    public static void EndInteraction(IInteractable target)
    {
        if (_currentInteraction == target)
        {
            _currentInteraction = null;
        }
    }

    public static bool IsInteracting => _currentInteraction != null;
}