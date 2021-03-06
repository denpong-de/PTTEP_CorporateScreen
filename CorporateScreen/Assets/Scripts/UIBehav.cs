using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehav : MonoBehaviour
{
    //For external class
    UIAnim uiAnim;

    //ScriptAbleObject
    [SerializeField] ConfigScriptableObject config;

    [SerializeField]GameObject masterCanvas;
    GameObject mainCanvas, aboutCanvas, videoCanvas, businessCanvas, 
        sustainCanvas, videoDCanvas, noInputPanel;
    List<GameObject> canvases = new List<GameObject>();

    int currentCanvas = 0;
    int closeCanvasIndex;

    void Start()
    {
        uiAnim = GetComponent<UIAnim>();

        //Subscribe to event.
        GameEvents.current.onStopScreenSaver += StartAnim;
        GameEvents.current.onStartScreenSaver += StopAnim;

        SetupValueble();
    }

    void SetupValueble()
    {
        //Get gameObject form master canvas
        mainCanvas = masterCanvas.transform.GetChild(0).gameObject;
        aboutCanvas = masterCanvas.transform.GetChild(1).gameObject;
        videoCanvas = masterCanvas.transform.GetChild(2).gameObject;
        businessCanvas = masterCanvas.transform.GetChild(3).gameObject;
        sustainCanvas = masterCanvas.transform.GetChild(4).gameObject;
        videoDCanvas = masterCanvas.transform.GetChild(5).gameObject;
        noInputPanel = masterCanvas.transform.GetChild(6).gameObject;

        //Add all canvas to canvases
        canvases.Add(mainCanvas);
        canvases.Add(aboutCanvas);
        canvases.Add(videoCanvas);
        canvases.Add(businessCanvas);
        canvases.Add(sustainCanvas);
        canvases.Add(videoDCanvas);
    }

    public void ChangeScene(int canvasIndex)
    {
        uiAnim.KillLoopSequence();

        switch (canvasIndex)
        {
            case 0:
                mainCanvas.SetActive(true);
                uiAnim.MainCanvasTween(2.5f);
                break;
            case 1:
                mainCanvas.SetActive(true); //For wake up form screen saver
                uiAnim.AboutCanvasTween(2.5f);
                currentCanvas = 1;
                closeCanvasIndex = 0;
                break;
            case 2:
                uiAnim.OurBusinessTween(2.5f);
                currentCanvas = 3;
                closeCanvasIndex = 2;
                break;
            case 3:
                mainCanvas.SetActive(true);
                uiAnim.OurBusinessTween(2.5f);
                currentCanvas = 3;
                closeCanvasIndex = 0;
                break;
            case 4:
                mainCanvas.SetActive(true);
                uiAnim.SusTween(2.5f);
                currentCanvas = 4;
                closeCanvasIndex = 0;
                config.curClip = 4;
                break;
            case 5:
                currentCanvas = 5;
                closeCanvasIndex = 0;
                break;
            case 6:
                currentCanvas = 2;
                closeCanvasIndex = 1;
                break;
            case 7:
                currentCanvas = 5;
                closeCanvasIndex = 3;
                break;
            case 8:
                currentCanvas = 5;
                closeCanvasIndex = 4;
                break;
            case 9:
                uiAnim.OurBusinessTween(2.5f);
                currentCanvas = 3;
                closeCanvasIndex = 5;
                break;
            case 10:
                uiAnim.SusTween(2.5f);
                currentCanvas = 4;
                closeCanvasIndex = 5;
                break;
            case 11:
                currentCanvas = 2;
                closeCanvasIndex = 3;
                break;
        }

        if(canvasIndex != 0)
        {
            //Make currentCanvas visible
            canvases[currentCanvas].transform.SetAsLastSibling();

            StartCoroutine(ChangeSceneDelay(canvases[currentCanvas], canvases[closeCanvasIndex], 2.5f));
            
            //Prevent form do another tween while tweening
            PreventInput();
        }
    }

    public void ReturnToMain()
    {
        uiAnim.KillLoopSequence();

        //Make main canvas visible
        mainCanvas.transform.SetAsLastSibling();

        if(currentCanvas != 0)
            StartCoroutine(ChangeSceneDelay(mainCanvas, canvases[currentCanvas], 2.5f));

        uiAnim.MainCanvasTween(2.5f);

        //Prevent form do another tween while tweening
        PreventInput();
        currentCanvas = 0;
    }

    void PreventInput()
    {
        noInputPanel.transform.SetAsLastSibling();
        StartCoroutine(ChangeSceneDelay(noInputPanel,noInputPanel,2.6f));
    }

    IEnumerator ChangeSceneDelay(GameObject newCanvas, GameObject oldCanvas,float duration)
    {
        newCanvas.SetActive(true);
        yield return new WaitForSeconds(duration);
        oldCanvas.SetActive(false);
    }

    void StartAnim()
    {
        //ChangeScene(currentCanvas);
        //PreventInput();
        if(currentCanvas == 0)
        {
            ChangeScene(currentCanvas);
            PreventInput();
        }
        else
        {
            ReturnToMain();
        }

    }

    void StopAnim()
    {
        uiAnim.KillLoopSequence();
    }

    private void OnDestroy()
    {
        //Unsubscribe to event.
        GameEvents.current.onStopScreenSaver -= StartAnim;
        GameEvents.current.onStartScreenSaver -= StopAnim;
    }
}
