using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/ConfigScriptableObject", order = 1)]
public class ConfigScriptableObject : ScriptableObject
{
    [Tooltip("How many seconds you have to wait before screen saver will be enable.")]
    public float ScreenSaverWaitTime;
}
