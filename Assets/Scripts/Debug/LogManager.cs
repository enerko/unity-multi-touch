using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private int _maxLines = 10;

    private readonly Queue<string> _logQueue = new();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            if (_logQueue.Count >= _maxLines)
                _logQueue.Dequeue();

            _logQueue.Enqueue(logString);

            _logText.text = string.Join("\n", _logQueue);
        }
    }
}
