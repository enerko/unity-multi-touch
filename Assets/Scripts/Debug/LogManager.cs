using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private int _maxLines = 10;

    private readonly Queue<string> _logQueue = new();

    public static LogManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    public void LogInfo(string category, string message)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{category}] [Info] {message}";
        Debug.Log(logEntry);
    }

    public void LogWarning(string category, string message)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{category}] [Warning] {message}";
        Debug.Log(logEntry);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string coloredLog;

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                coloredLog = $"<color=red>{logString}</color>";
                break;
            case LogType.Warning:
                coloredLog = $"<color=yellow>{logString}</color>";
                break;
            default:
                coloredLog = logString;
                break;
        }

        if (_logQueue.Count >= _maxLines)
            _logQueue.Dequeue();

        _logQueue.Enqueue(logString);
        _logText.text = string.Join("\n", _logQueue);
    }
}
