using UnityEngine;

/* In ScreenSaver_Canvas, It had button component when user touch the screen.
 * it will SetActive(false) itself and call StopScreenSaver()
 * and this code will run */
public class ScreenSaverBehav : MonoBehaviour
{
    //ScriptAbleObject
    [SerializeField] ConfigScriptableObject config;

    [SerializeField] GameObject ScreenSaverCanvas;
    float LastIdleTime;
    bool isScreenSaver = true;

    void Start()
    {
        //Wait for event finished initialize
        Invoke("CallEvent",0.3f);
    }

    void Update()
    {
        //If ScreenSaver curently play do nothing
        if(!isScreenSaver)
            IdleCheck();
    }

    void IdleCheck()
    {
        //Enable ScreenSaver Canvas & disable this gameObject(ScreenSaver Manager)
        //You can config ScreenSaver wait time in Assets>Config
        if (Input.GetButton("Fire1"))
        {
            LastIdleTime = Time.time;
        }
        else if (Time.time - LastIdleTime > config.ScreenSaverWaitTime)
        {
            ScreenSaverCanvas.SetActive(true);
            ScreenSaverCanvas.transform.SetAsLastSibling();

            isScreenSaver = true;

            //Call onStartScreenSaver event
            GameEvents.current.StartScreenSaver();
        }
    }

    public void StopScreenSaver()
    {
        //Make IdleCheck() working
        isScreenSaver = false;

        //Call onStopScreenSaver event
        GameEvents.current.StopScreenSaver();

        //Reset Last idle time
        LastIdleTime = Time.time;
    }

    private void CallEvent()
    {
        //Call onStartScreenSaver event
        GameEvents.current.StartScreenSaver();
    }
}
