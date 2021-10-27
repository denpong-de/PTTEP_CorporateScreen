using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OurBusinessVideo : MonoBehaviour
{
    //For external class
    VideoBehav videoBehav;

    [SerializeField] RenderTexture renderTexture;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] VideoClip[] videoClipLoops;
    [SerializeField] GameObject controlButtonCanvas;
    [SerializeField] GameObject[] onVideoButtons;
    Button nextButton, previousButton;

    int curClip;

    void Start()
    {
        videoBehav = GetComponent<VideoBehav>();

        //Subscribe to event.
        videoPlayer.loopPointReached += EndReached;

        //Get Button form controlButton canvas
        nextButton = controlButtonCanvas.transform.GetChild(0).GetComponent<Button>();
        previousButton = controlButtonCanvas.transform.GetChild(1).GetComponent<Button>();
    }

    //Play loop clip when video end
    void EndReached(VideoPlayer videoPlayer)
    {
        //if there is no loop version of that clip do nothing
        if (videoClipLoops[curClip] == null) return;

        //Play current clip but loop version
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

            //for WebGL
            //videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "AWARDS.mp4");
            //videoPlayer.Play();
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
