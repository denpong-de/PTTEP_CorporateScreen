using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onStartScreenSaver;
    public void StartScreenSaver()
    {
        if (onStartScreenSaver != null)
        {
            onStartScreenSaver();
        }
    }

    public event Action onStopScreenSaver;
    public void StopScreenSaver()
    {
        if (onStopScreenSaver != null)
        {
            onStopScreenSaver();
        }
    }
}
