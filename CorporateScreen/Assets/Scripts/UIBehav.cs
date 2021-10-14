using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehav : MonoBehaviour
{
    UIAnim uiAnim;

    [SerializeField]GameObject masterCanvas;
    GameObject mainCanvas, aboutCanvas, videoCanvas, businessCanvas, sustainCanvas, videoDCanvas;
    List<GameObject> canvases = new List<GameObject>();

    int currentCanvas = 1;
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

        canvases.Add(mainCanvas);
        canvases.Add(aboutCanvas);
        canvases.Add(videoCanvas);
        canvases.Add(businessCanvas);
        canvases.Add(sustainCanvas);
    }

    public void ChangeScene(int canvasIndex)
    {
        uiAnim.KillLoopSequence();

        switch (canvasIndex)
        {
            case 1:
                mainCanvas.SetActive(true); //For wake up form screen saver
                uiAnim.AboutCanvasTween(2.5f);
                currentCanvas = 1;
                closeCanvasIndex = 0;
                break;
            case 2:
                currentCanvas = 3;
                closeCanvasIndex = 2;
                break;
            case 3:
                currentCanvas = 3;
                closeCanvasIndex = 0;
                break;
            case 4:
                currentCanvas = 4;
                closeCanvasIndex = 0;
                break;
        }

        canvases[currentCanvas].transform.SetAsLastSibling();
        StartCoroutine(ChangeSceneDelay(canvases[currentCanvas], canvases[closeCanvasIndex], 2.5f));
    }

    public void ReturnToMain()
    {
        mainCanvas.transform.SetAsLastSibling();
        StartCoroutine(ChangeSceneDelay(mainCanvas, canvases[currentCanvas], 2.5f));
        if(currentCanvas >= 2)
            StartCoroutine(ChangeSceneDelay(mainCanvas, videoDCanvas, 2.5f));
        uiAnim.MainCanvasTween(2.5f);
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
