using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehav : MonoBehaviour
{
    UIAnim uiAnim;

    [SerializeField]GameObject canvas;
    GameObject mainCanvas, aboutCanvas, homeButton;

    void Start()
    {
        uiAnim = GetComponent<UIAnim>();

        //Subscribe to event.
        GameEvents.current.onStopScreenSaver += StartAnim;
        GameEvents.current.onStartScreenSaver += StopAnim;

        //Setup Canvas
        mainCanvas = canvas.transform.GetChild(0).gameObject;
        aboutCanvas = canvas.transform.GetChild(1).gameObject;
        homeButton = canvas.transform.GetChild(2).gameObject;

        //uiAnim.AboutCanvasTween(2.5f);
    }

    public void ChangeScene(int canvasIndex)
    {
        uiAnim.KillLoopSequence();

        switch (canvasIndex)
        {
            case 1:
                aboutCanvas.transform.SetAsFirstSibling();
                StartCoroutine(ChangeSceneDelay(mainCanvas,aboutCanvas,2.5f));
                homeButton.SetActive(false);
                uiAnim.MainCanvasTween(2.5f);
                break;
            case 2:
                mainCanvas.transform.SetAsFirstSibling();
                StartCoroutine(ChangeSceneDelay(aboutCanvas, mainCanvas, 2.5f));
                homeButton.SetActive(true);
                uiAnim.AboutCanvasTween(1f);
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
        uiAnim.MainCanvasTween(2.5f);
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
