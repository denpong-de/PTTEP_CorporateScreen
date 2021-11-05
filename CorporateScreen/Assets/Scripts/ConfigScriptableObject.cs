using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/ConfigScriptableObject", order = 1)]
public class ConfigScriptableObject : ScriptableObject
{
    [Header("Config")]
    [Tooltip("How many seconds you have to wait before screen saver will be enable.")]
    public float ScreenSaverWaitTime;

    [Header("Realtime Database (Don't change any value)")]
    [Tooltip("Current clip index of OurBusinessVideo")]
    public int curClip;
}
