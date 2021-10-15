using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehav : MonoBehaviour
{
    UIAnim uiAnim;

    [SerializeField]GameObject masterCanvas;
    GameObject mainCanvas, aboutCanvas, videoCanvas, businessCanvas, sustainCanvas, videoDCanvas, noInputPanel;
    List<GameObject> canvases = new List<GameObject>();

    int currentCanvas = 0;
    int closeCanvasIndex;

    void Start()
    {
        uiAnim = GetComponent<UIAnim>();

        //Subscribe to event.
        GameEvents.current.onStopScreenSaver += StartAnim;
        GameEvents.current.onStartScreenSaver += StopAnim;

        //Setup valueble
        mainCanvas = masterCanvas.transform.GetChild(0).gameObject;
        aboutCanvas = masterCanvas.transform.GetChild(1).gameObject;
        videoCanvas = masterCanvas.transform.GetChild(2).gameObject;
        businessCanvas = masterCanvas.transform.GetChild(3).gameObject;
        sustainCanvas = masterCanvas.transform.GetChild(4).gameObject;
        videoDCanvas = masterCanvas.transform.GetChild(5).gameObject;
        noInputPanel = masterCanvas.transform.GetChild(6).gameObject;

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
        }

        if(canvasIndex != 0)
        {
            canvases[currentCanvas].transform.SetAsLastSibling();
            StartCoroutine(ChangeSceneDelay(canvases[currentCanvas], canvases[closeCanvasIndex], 2.5f));
            PreventInput();
        }

    }

    public void ReturnToMain()
    {
        mainCanvas.transform.SetAsLastSibling();
        if(currentCanvas != 0)
        {
            StartCoroutine(ChangeSceneDelay(mainCanvas, canvases[currentCanvas], 2.5f));
        }
        if (videoCanvas.gameObject.activeInHierarchy)
        {
            StartCoroutine(ChangeSceneDelay(mainCanvas, videoDCanvas, 2.5f));
        }
        uiAnim.MainCanvasTween(2.5f);
        PreventInput();
        currentCanvas = 0;
    }

    void PreventInput()
    {
        if(currentCanvas == 5)
        {
            return;
        }

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
        ChangeScene(currentCanvas);
        PreventInput();
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
