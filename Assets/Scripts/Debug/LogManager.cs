using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private int _maxLines = 10;

    private readonly Queue<string> _logQueue = new();

    public static LogManager Instance { get; private set; }

    private string _logFilePath;

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
        _logFilePath = Path.Combine(Application.persistentDataPath, "log.txt");
        File.WriteAllText(_logFilePath, string.Empty); // Clear file at session start

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
        Debug.LogWarning(logEntry);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (_logQueue.Count >= _maxLines)
            _logQueue.Dequeue();

        _logQueue.Enqueue(logString);

        string logText = string.Join("\n", _logQueue);
        _logText.text = logText;

        // Save logs on a persistent file. Show stack traces if it is a type of warning, error, or exception,
        string logEntry = logString;

        if (type == LogType.Warning || type == LogType.Error || type == LogType.Exception)
            logEntry += "\n" + stackTrace;

        logEntry += "\n";

        File.AppendAllText(_logFilePath, logEntry);
    }
}
