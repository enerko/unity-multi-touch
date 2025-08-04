using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private int _maxLines = 15;

    private Queue<string> _logLines = new Queue<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Log(string message)
    {
        if (_logLines.Count >= _maxLines)
        {
            _logLines.Dequeue();
        }

        _logLines.Enqueue(message);
        _logText.text = string.Join("\n", _logLines);
    }
}
