using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class UseJS : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Alert();

    [DllImport("__Internal")]
    private static extern void OpenPopup();

    public void UnityCallJSFunc()
    {
        Alert();
    }

    public void OpenWebPopup()
    {
        OpenPopup();
    }
}
