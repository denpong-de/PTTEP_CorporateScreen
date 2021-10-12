using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class VideoBehav : MonoBehaviour
{
    [SerializeField] GameObject videoCanvas;
    [SerializeField] RectTransform videoRowCanvas;
    [SerializeField] RectTransform[] videoPanels;
    [SerializeField] VideoPlayer[] videoPlayers; 
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] VideoClip[] videoClipLoops;
    [SerializeField] RenderTexture[] renderTextures;

    //Current videoClips[] index
    int curClip;
    float videoRowCanvasPosY;
    int panelSwitchCount;

    void Start()
    {
        //Subscribe all video players to EndReached method when start program
        foreach (var videoPlayer in videoPlayers)
        {
            videoPlayer.loopPointReached += EndReached;
        }

        //Debug
        PlayVideo(0);
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
        videoPlayer.clip = videoClipLoops[curClip];
        videoPlayer.isLooping = true;
        videoPlayer.Play();
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

        //Make sure video canvas is visible
        videoCanvas.transform.SetAsLastSibling();
        videoCanvas.SetActive(true);

        if(index == 0)
        {
            //Play first videoClips in videoPlayers[0]
            ChangeVideo(videoPlayers[0], videoClips[0], false);
        }
        else if(index > 0 && index < videoClips.Length -1)
        {
            //Tween videoRowCanvas to VideoB Panel
            videoRowCanvasPosY = videoRowCanvas.anchoredPosition.y + 2161.01f;
            videoRowCanvas.DOAnchorPos(new Vector2(0, videoRowCanvasPosY), 0);

            //Play videoClips[index] in videoPlayers[1]
            ChangeVideo(videoPlayers[1], videoClips[index], false);
        }
        else if (index == videoClips.Length -1)
        {
            //Tween videoRowCanvas to VideoC Panel
            videoRowCanvasPosY = videoRowCanvas.anchoredPosition.y + 4322.02f;
            videoRowCanvas.DOAnchorPos(new Vector2(0, videoRowCanvasPosY), 0);

            //Play last videoClips in videoPlayers[2]
            ChangeVideo(videoPlayers[2], videoClips[index], false);
        }
    }

    //When Pressed down button in program
    public void NextVideo()
    {
        curClip++;

        videoRowCanvasPosY = videoRowCanvas.anchoredPosition.y + 2161.01f;

        if (curClip == 1)
        {
            videoRowCanvas.DOAnchorPos(new Vector2(0, videoRowCanvasPosY), 2.5f);
            int previousPlayer = curClip - 1;
            StartCoroutine(ClosePreviousVideo(previousPlayer, 2.5f));
        }
        else
        {
            videoRowCanvas.DOAnchorPos(new Vector2(0, videoRowCanvasPosY), 2.5f)
                .OnComplete(SwitchPanel);
        }

        NextVideoPlay();
    }

    //VideoPlayer Behav
    void NextVideoPlay()
    {
        //if (curClip % 3 == 1)
        //{
        //    ChangeVideo(videoPlayers[1], videoClips[curClip], false);
        //}
        //if (curClip % 3 == 2)
        //{
        //    ChangeVideo(videoPlayers[2], videoClips[curClip], false);
        //}
        //if (curClip % 3 == 0)
        //{
        //    ChangeVideo(videoPlayers[0], videoClips[curClip], false);
        //}

        int curPlayer = curClip % 3;
        ChangeVideo(videoPlayers[curPlayer], videoClips[curClip], false);
    }

    void ChangeVideo(VideoPlayer videoPlayer, VideoClip clip, bool isLooping)
    {
        videoPlayer.clip = clip;
        videoPlayer.isLooping = isLooping;
        videoPlayer.Play();
    }

    //Make the first panel goto last 
    void SwitchPanel()
    {
        int panelThatSwitch;
        int previousPlayer;

        panelSwitchCount++;
        
        switch (panelSwitchCount % 3)
        {
            case 1:
                panelThatSwitch = 0;
                previousPlayer = 1;
                break;
            case 2:
                panelThatSwitch = 1;
                previousPlayer = 2;
                break;
            default:
                panelThatSwitch = 2;
                previousPlayer = 0;
                break;
        }

        float stopPanelPosY = videoPanels[panelThatSwitch].anchoredPosition.y - 6483.03f;
        videoPanels[panelThatSwitch].DOAnchorPos(new Vector2(1920, stopPanelPosY), 0);
         
        StartCoroutine(ClosePreviousVideo(previousPlayer, 2.5f));
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
    }

    //clear render texture
    void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
}
