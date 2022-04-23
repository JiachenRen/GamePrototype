using System;
using UnityEngine;

public class GameState: MonoBehaviour
{
    private static GameState _eventManager;

    private bool initialized;

    public bool playing;

    public static GameState instance
    {
        get
        {
            if (_eventManager) return _eventManager;
            _eventManager = FindObjectOfType(typeof(GameState)) as GameState;

            if (!_eventManager)
                Debug.LogError("There needs to be one active GameState script on a GameObject in your scene.");
            else
                _eventManager.Init();

            return _eventManager;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance.initialized) {
            Destroy(gameObject);
            return;
        }

        Init();
        initialized = true;
    }

    private void Init()
    {
        
    }

    public static void TogglePlay()
    {
        if (instance.playing)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public static void Pause()
    {
        instance.playing = false;
        UpdateCursorLockState();
    }

    public static void Resume()
    {
        instance.playing = true;
        UpdateCursorLockState();
    }

    private static void UpdateCursorLockState()
    {
        Cursor.lockState = instance.playing ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
