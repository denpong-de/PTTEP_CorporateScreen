using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class VideoBehav : MonoBehaviour
{
    [SerializeField] UIBehav uiBehav;

    [SerializeField] GameObject videoCanvas;
    [SerializeField] RectTransform videoRowCanvas;
    [SerializeField] RectTransform[] videoPanels;
    [SerializeField] VideoPlayer[] videoPlayers; 
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] VideoClip[] videoClipLoops;
    [SerializeField] RenderTexture[] renderTextures;
    [SerializeField] Button nextButton;
    [SerializeField] Button previousButton;

    [SerializeField] Button homeButton;

    //Current videoClips[] index
    int curClip;
    int curPlayerReset = 16;
    int curPlayer = 16;
    float videoRowCanvasPosY;

    Vector2 videoRowCanvasStartPos;
    List<float> videoPanelStartPos = new List<float>();

    bool isSkipFrame;

    void Start()
    {
        //Subscribe all video players to EndReached method when start program
        foreach (var videoPlayer in videoPlayers)
        {
            videoPlayer.loopPointReached += EndReached;
        }

        SaveStartPos();
    }

    //Play loop clip when video end
    void EndReached(VideoPlayer videoPlayer)
    {
        #if(UNITY_EDITOR)
        Debug.Log("End reached");
        #endif

        //if there is no loop version of that clip do nothing
        if (videoClipLoops[curClip] == null) return;

        //Play current clip but loop version
        //videoPlayer.clip = videoClipLoops[curClip];
        //videoPlayer.isLooping = true;
        //videoPlayer.Play();

        ChangeVideo(videoPlayer,videoClipLoops[curClip],true);
    }

    void SaveStartPos()
    {
        videoRowCanvasStartPos = videoRowCanvas.anchoredPosition;

        for (int i = 0; i < videoPanels.Length; i++)
        {
            float startPos = videoPanels[i].anchoredPosition.y;
            videoPanelStartPos.Add(startPos);
        }
    }

    //Unsubscribe all video players to EndReached method when close programs
    private void OnDestroy()
    {
        foreach (var videoPlayer in videoPlayers)
        {
            videoPlayer.loopPointReached -= EndReached;
        }
    }

    //When clicked in main menu
    public void PlayVideo(int index)
    {
        curClip = index;
        curPlayer = curPlayerReset;

        //Make sure video canvas is visible
        //videoCanvas.transform.SetAsLastSibling();
        //videoCanvas.SetActive(true);

        CheckButton();

        //Play videoClips[index] in videoPlayers[1]
        ChangeVideo(videoPlayers[1], videoClips[index], false);
    }

    //When Pressed down button in program
    public void NextVideo()
    {
        //if it is the last video change to OurBusiness canvas
        if(curClip == videoClips.Length -1)
        {
            uiBehav.ChangeScene(2);
            return;
        }

        curClip++;

        if(curClip != 4)
        {
            curPlayer++;
            videoRowCanvasPosY = videoRowCanvas.anchoredPosition.y + 2161.01f;

            DisableButton(false);

            //Move to next panel
            videoRowCanvas.DOAnchorPos(new Vector2(0, videoRowCanvasPosY), 2.5f)
                .OnComplete(SwitchPanelDown);
        }
        isSkipFrame = false;

        //Play video on next panel
        NextVideoPlay();
    }

    //When Pressed up button in program
    public void PreviousVideo()
    {
        curClip--;

        if (curClip != 3)
        {
            curPlayer--;
            isSkipFrame = false;
            videoRowCanvasPosY = videoRowCanvas.anchoredPosition.y - 2161.01f;

            DisableButton(false);

            videoRowCanvas.DOAnchorPos(new Vector2(0, videoRowCanvasPosY), 2.5f)
                .OnComplete(SwitchPanelUp);
        }
        else
        {
            isSkipFrame = true;
        }

        //Play video on next panel
        NextVideoPlay();
    }

    void DisableButton(bool isEnable)
    {
        nextButton.interactable = isEnable;
        previousButton.interactable = isEnable;
        homeButton.interactable = isEnable;
    }

    void CheckButton()
    {

        if (curClip == 0)
        {
            previousButton.interactable = false;
            Debug.Log("Disable up");
        }
        else
        {
            previousButton.interactable = true;
            nextButton.interactable = true;
            Debug.Log("Not Disable");
        }
    }

    //VideoPlayer Behav
    void NextVideoPlay()
    {
        ChangeVideo(videoPlayers[curPlayer % 3], videoClips[curClip], false);
    }

    public void ChangeVideo(VideoPlayer videoPlayer, VideoClip clip, bool isLooping)
    {
        videoPlayer.clip = clip;
        videoPlayer.isLooping = isLooping;
        videoPlayer.Play();

        if (isSkipFrame)
        {
            videoPlayer.frame = 80;
        }
    }

    //Make the first panel goto last 
    void SwitchPanelDown()
    {
        int panelThatSwitch;
        int previousPlayer;

        switch (curPlayer % 3)
        {
            case 1:
                panelThatSwitch = 2;
                previousPlayer = 0;
                break;
            case 2:
                panelThatSwitch = 0;
                previousPlayer = 1;
                break;
            default:
                panelThatSwitch = 1;
                previousPlayer = 2;
                break;
        }

        float stopPanelPosY = videoPanels[panelThatSwitch].anchoredPosition.y - 6483.03f;
        videoPanels[panelThatSwitch].DOAnchorPos(new Vector2(1920, stopPanelPosY), 0);

        StartCoroutine(ClosePreviousVideo(previousPlayer, 0));

        DisableButton(true);

        //if it is the last clip disable newt button
        CheckButton();
    }

    //Make the last panel goto first 
    void SwitchPanelUp()
    {
        int panelThatSwitch;
        int previousPlayer;

        switch (curPlayer % 3)
        {
            case 1:
                panelThatSwitch = 0;
                previousPlayer = 2;
                break;
            case 2:
                panelThatSwitch = 1;
                previousPlayer = 0;
                break;
            default:
                panelThatSwitch = 2;
                previousPlayer = 1;
                break;
        }

        float stopPanelPosY = videoPanels[panelThatSwitch].anchoredPosition.y + 6483.03f;
        videoPanels[panelThatSwitch].DOAnchorPos(new Vector2(1920, stopPanelPosY), 0);

        StartCoroutine(ClosePreviousVideo(previousPlayer, 0));

        DisableButton(true);

        //if it is the last clip disable newt button
        CheckButton();
    }

    //Close previous player when tween is finished 
    IEnumerator ClosePreviousVideo(int videoPlayer ,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        videoPlayers[videoPlayer].Stop();
        ClearOutRenderTexture(renderTextures[videoPlayer]);
    }

    //When pressed home stop all video players and clear all render textures
    public void CloseAllVideoCanvas()
    {
        //stop all video players
        foreach (var videoPlayer in videoPlayers)
        {
            if (videoPlayer != null)
                videoPlayer.Stop();
        }

        //clear all render textures
        foreach (var renderTexture in renderTextures)
        {
            ClearOutRenderTexture(renderTexture);
        }

        isSkipFrame = false;
        ResetCanvasPos();
    }

    //clear render texture
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

    void ResetCanvasPos()
    {
        videoRowCanvas.anchoredPosition = videoRowCanvasStartPos;

        for (int i = 0; i < videoPanels.Length; i++)
        {
            videoPanels[i].DOAnchorPos(new Vector2(1920,videoPanelStartPos[i]),0f);
        }
    }
}
