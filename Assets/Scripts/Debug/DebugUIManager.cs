using UnityEngine;

public class DebugUIManager : MonoBehaviour
{
    public void ToggleVisualizeTouch()
    {
        DebugParameters.IsVisualizeTouch = !DebugParameters.IsVisualizeTouch;
        LogManager.Instance.LogInfo("Debug", $"Turned IsVisualTouch {DebugParameters.IsVisualizeTouch}");
    }
}