using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class VideoBehav : MonoBehaviour
{
    //For external class
    [SerializeField] UIBehav uiBehav;

    [SerializeField] RectTransform videoRowCanvas;
    [SerializeField] RectTransform[] videoPanels;
    [SerializeField] VideoPlayer[] videoPlayers; 
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] VideoClip[] videoClipLoops;
    [SerializeField] RenderTexture[] renderTextures;
    [SerializeField] Button nextButton;
    [SerializeField] Button previousButton;
    [SerializeField] Button homeButton;

    //For slide tween
    int curClip, curPlayer;
    float videoRowCanvasPosX;
    //For reset position when pressed home or change canvas
    float videoRowCanvasStartPos;
    List<float> videoPanelStartPos = new List<float>();
    //For playing video in a same panel
    bool isSkipFrame;

    void Start()
    {
        //Subscribe all video players to EndReached method when start program
        foreach (var videoPlayer in videoPlayers)
        {
            videoPlayer.loopPointReached += EndReached;
        }

        //Save all video players initial position for reset video players position
        SaveStartPos();
    }

    //Play loop clip when video end
    void EndReached(VideoPlayer videoPlayer)
    {
        //if there is no loop version of that clip do nothing
        if (videoClipLoops[curClip] == null) return;

        //Play current clip but loop version
        ChangeVideo(videoPlayer,videoClipLoops[curClip],true);
    }

    void SaveStartPos()
    {
        //Save Row Canvas initial position
        videoRowCanvasStartPos = videoRowCanvas.anchoredPosition.x;

        //Save all videoPanels initial position
        for (int i = 0; i < videoPanels.Length; i++)
        {
            float startPos = videoPanels[i].anchoredPosition.x;
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
        //Play on VideoB Panel
        curPlayer = 1;

        //If it's first clip disable previous button else enable it
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
            //Reset video canvas and it child to initial state
            CloseAllVideoCanvas();
            return;
        }

        //Else Play next clip
        curClip++;
        isSkipFrame = false;

        //If it's clip 2 or 5 just play clip else tween canvas down & play clip
        if (curClip == 2 || curClip == 5)
        {
            //do nothing
        }
        else
        {
            //Next Player
            curPlayer++;
            curPlayer = curPlayer % 3;

            //Prevent form do another tween while tweening
            DisableButton(false);

            videoRowCanvasPosX = videoRowCanvas.anchoredPosition.x - 3840f;

            //Move to next panel when finished SwitchPanelDown
            videoRowCanvas.DOAnchorPos(new Vector2(videoRowCanvasPosX, 0), 2.5f)
                .OnComplete(SwitchPanelDown);
        }

        //Play video on next panel
        NextVideoPlay();
    }

    //When Pressed up button in program
    public void PreviousVideo()
    {
        curClip--;

        //If it's not clip 4 tween canvas down else just play clip
        if (curClip == 1 || curClip == 4)
        {
            if(curClip == 4)
            {
                //Skip intro of a clip for more fluid transition 
                isSkipFrame = true;
            }
        }
        else
        {
            //Switch to previous player
            switch (curPlayer)
            {
                case 1:
                    curPlayer = 0;
                    break;
                case 2:
                    curPlayer = 1;
                    break;
                default:
                    curPlayer = 2;
                    break;
            }

            //Prevent form do another tween while tweening
            DisableButton(false);
            isSkipFrame = false;

            videoRowCanvasPosX = videoRowCanvas.anchoredPosition.x + 3840f;

            //Move to previous panel when finished SwitchPanelUp
            videoRowCanvas.DOAnchorPos(new Vector2(videoRowCanvasPosX, 0), 2.5f)
                .OnComplete(SwitchPanelUp);
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
        switch (curClip)
        {
            case 0:
                previousButton.interactable = false;
                break;
            default:
                previousButton.interactable = true;
                nextButton.interactable = true;
                break;
        }
    }

    //VideoPlayer Behavior
    void NextVideoPlay()
    {
        ChangeVideo(videoPlayers[curPlayer], videoClips[curClip], false);
    }

    public void ChangeVideo(VideoPlayer videoPlayer, VideoClip clip, bool isLooping)
    {
        videoPlayer.clip = clip;
        videoPlayer.isLooping = isLooping;
        videoPlayer.Play();

        if (isSkipFrame)
            videoPlayer.frame = 80;
    }

    //Make the first panel goto last 
    void SwitchPanelDown()
    {
        int panelThatSwitch;
        int previousPlayer;

        switch (curPlayer)
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

        float stopPanelPosX = videoPanels[panelThatSwitch].anchoredPosition.x + 11520;

        TweenPanel(panelThatSwitch, previousPlayer, stopPanelPosX);
    }

    //Make the last panel goto first 
    void SwitchPanelUp()
    {
        int panelThatSwitch;
        int previousPlayer;

        switch (curPlayer)
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

        float stopPanelPosX = videoPanels[panelThatSwitch].anchoredPosition.x - 11520;

        TweenPanel(panelThatSwitch, previousPlayer, stopPanelPosX);
    }

    void TweenPanel(int panelThatSwitch, int previousPlayer,float stopPanelPosX)
    {
        videoPanels[panelThatSwitch].DOAnchorPos(new Vector2(stopPanelPosX, -1080), 0);

        ClosePreviousVideo(previousPlayer);

        //Enable buttons
        DisableButton(true);

        //if it is the last clip disable newt button
        CheckButton();
    }

    //Close previous player when tween is finished 
    void ClosePreviousVideo(int videoPlayer)
    {
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

        //Reset bool & canvas position
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
        videoRowCanvas.DOAnchorPos(new Vector2(videoRowCanvasStartPos, 0),0);

        for (int i = 0; i < videoPanels.Length; i++)
        {
            videoPanels[i].DOAnchorPos(new Vector2(videoPanelStartPos[i], -1080),0);
        }
    }
}
