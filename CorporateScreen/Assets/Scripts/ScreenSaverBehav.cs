using UnityEngine;

/* In ScreenSaver_Canvas, It had button component when user touch the screen.
 * it will SetActive(false) itself and SetActive(true) ScreenSaver_Manager
 * and this code will run */
public class ScreenSaverBehav : MonoBehaviour
{
    //ScriptAbleObject
    [SerializeField] ConfigScriptableObject config;

    [SerializeField] GameObject ScreenSaverCanvas;
    float LastIdleTime;

    void OnEnable()
    {
        //Call onStopScreenSaver event
        GameEvents.current.StopScreenSaver();

        LastIdleTime = Time.time;
    }

    void OnDisable()
    {
        //Call onStartScreenSaver event
        GameEvents.current.StartScreenSaver();
    }

    void Update()
    {
        IdleCheck();
    }

    void IdleCheck()
    {
        #if (UNITY_EDITOR)
        Debug.Log(Time.time - LastIdleTime + ":" + config.ScreenSaverWaitTime);
        #endif

        if(Time.time - LastIdleTime > config.ScreenSaverWaitTime)
        {
            ScreenSaverCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
