using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OurBusinessVideo : MonoBehaviour
{
    VideoBehav videoBehav;

    [SerializeField] RenderTexture renderTexture;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] VideoClip[] videoClipLoops;
    [SerializeField] GameObject controlButtonCanvas;
    [SerializeField] GameObject[] onVideoButtons;
    Button nextButton, previousButton;

    int curClip;

    // Start is called before the first frame update
    void Start()
    {
        videoBehav = GetComponent<VideoBehav>();

        videoPlayer.loopPointReached += EndReached;

        nextButton = controlButtonCanvas.transform.GetChild(0).GetComponent<Button>();
        previousButton = controlButtonCanvas.transform.GetChild(1).GetComponent<Button>();
    }

    void EndReached(VideoPlayer videoPlayer)
    {
        //if there is no loop version of that clip do nothing
        if (videoClipLoops[curClip] == null) return;

        //Play current clip but loop version
        //videoPlayer.clip = videoClipLoops[curClip];
        //videoPlayer.isLooping = true;
        //videoPlayer.Play();

        videoBehav.ChangeVideo(videoPlayer, videoClipLoops[curClip], true);
    }

    //Unsubscribe all video players to EndReached method when close programs
    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= EndReached;
    }

    public void PlayVideo(int index)
    {
        curClip = index;

        //videoPanel.SetActive(true);
        //videoPanel.transform.SetAsLastSibling();

        if (curClip == 0 || curClip == 3)
        {
            if (curClip == 0)
            {
                previousButton.interactable = false;
                onVideoButtons[0].SetActive(true);
            }     
            
            controlButtonCanvas.SetActive(true);
        }
        else if (curClip >= 4 && curClip < 8)
        {
            controlButtonCanvas.SetActive(true);
            nextButton.interactable = true;
            previousButton.interactable = true;
        }
        else if (curClip == 8)
        {
            controlButtonCanvas.SetActive(true);
            nextButton.interactable = false;
        }
        else
        {
            controlButtonCanvas.SetActive(false);
        }
            
        videoBehav.ChangeVideo(videoPlayer,videoClips[curClip],false);
    }

    public void NextVideo()
    {
        curClip++;

        if (curClip == 1)
        {
            previousButton.interactable = true;
            curClip = 3;
        }   
        else if (curClip == 4)
        {
            Debug.Log("Load sustain");
        }
        else if (curClip == 8)
        {
            nextButton.interactable = false;
        } 
        else
        {
            nextButton.interactable = true;
            previousButton.interactable = true;
        }

        CheckOnVideoButton();

        videoBehav.ChangeVideo(videoPlayer, videoClips[curClip], false);
    }

    public void PreviousVideo()
    {
        curClip--;

        if (curClip == 2)
        {
            previousButton.interactable = false;
            curClip = 0;
        }   
        else
        {
            nextButton.interactable = true;
            previousButton.interactable = true;
        }

        CheckOnVideoButton();

        videoBehav.ChangeVideo(videoPlayer, videoClips[curClip], false);
    }

    void CheckOnVideoButton()
    {
        if(curClip == 0)
        {
            onVideoButtons[0].SetActive(true);
            onVideoButtons[1].SetActive(false);
        }
        else if (curClip == 3)
        {
            onVideoButtons[1].SetActive(true);
        }
        else
        {
            onVideoButtons[0].SetActive(false);
            onVideoButtons[1].SetActive(false);
        }
    }

    public void ClearVideo()
    {
        videoPlayer.Stop();
        videoBehav.ClearOutRenderTexture(renderTexture);
    }
}
