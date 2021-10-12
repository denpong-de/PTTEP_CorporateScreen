using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehav : MonoBehaviour
{
    UIAnim uiAnim;

    [SerializeField]GameObject canvas;
    GameObject mainCanvas, aboutCanvas, videoCanvas, homeButton;

    int currentCanvas = 1;

    void Start()
    {
        uiAnim = GetComponent<UIAnim>();

        //Subscribe to event.
        GameEvents.current.onStopScreenSaver += StartAnim;
        GameEvents.current.onStartScreenSaver += StopAnim;

        //Setup valueble
        mainCanvas = canvas.transform.GetChild(0).gameObject;
        aboutCanvas = canvas.transform.GetChild(1).gameObject;
        videoCanvas = canvas.transform.GetChild(2).gameObject;
        homeButton = canvas.transform.GetChild(3).gameObject;
    }

    public void ChangeScene(int canvasIndex)
    {
        uiAnim.KillLoopSequence();

        switch (canvasIndex)
        {
            case 1:
                mainCanvas.transform.SetAsLastSibling();
                StartCoroutine(ChangeSceneDelay(mainCanvas,aboutCanvas,2.5f));
                uiAnim.MainCanvasTween(2.5f);
                currentCanvas = 1;
                break;
            case 2:
                mainCanvas.SetActive(true); //For wake up form screen saver
                aboutCanvas.transform.SetAsLastSibling();
                StartCoroutine(ChangeSceneDelay(aboutCanvas, mainCanvas, 2.5f));
                uiAnim.AboutCanvasTween(2.5f);
                currentCanvas = 2;
                break;
            case 3:
                mainCanvas.transform.SetAsLastSibling();
                StartCoroutine(ChangeSceneDelay(mainCanvas,videoCanvas,2.5f));
                uiAnim.MainCanvasTween(2.5f);
                currentCanvas = 1;
                break;
        }
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
