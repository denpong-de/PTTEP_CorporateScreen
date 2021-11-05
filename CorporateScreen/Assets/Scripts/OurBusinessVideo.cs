using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OurBusinessVideo : MonoBehaviour
{
    //For external class
    VideoBehav videoBehav;
    [SerializeField] UIBehav uiBehav;

    //ScriptAbleObject
    [SerializeField] ConfigScriptableObject config;

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
        nextButton = controlButtonCanvas.transform.GetChild(1).GetComponent<Button>();
        previousButton = controlButtonCanvas.transform.GetChild(2).GetComponent<Button>();
    }

    //Play loop clip when video end
    void EndReached(VideoPlayer videoPlayer)
    {
        //if there is no loop version of that clip do nothing
        if (videoClipLoops[config.curClip] == null) return;

        //Play current clip but loop version
        videoBehav.ChangeVideo(videoPlayer, videoClipLoops[config.curClip], true);
    }

    //Unsubscribe all video players to EndReached method when close programs
    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= EndReached;
    }

    public void PlayVideo(int index)
    {
        config.curClip = index;

        if (config.curClip == 0 || config.curClip == 3)
        {
            if (config.curClip == 0)
            {
                onVideoButtons[0].SetActive(true);
            }     
            
            controlButtonCanvas.SetActive(true);
        }
        else if (config.curClip >= 4 && config.curClip < 9)
        {
            controlButtonCanvas.SetActive(true);
            nextButton.interactable = true;
            previousButton.interactable = true;
        }
        else if (config.curClip == 9)
        {
            controlButtonCanvas.SetActive(true);
            previousButton.interactable = true;
            nextButton.interactable = false;

            //for WebGL
            //videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "AWARDS.mp4");
            //videoPlayer.Play();
        }
        else
        {
            //controlButtonCanvas.SetActive(false);
        }

        videoBehav.ChangeVideo(videoPlayer,videoClips[config.curClip],false);
    }

    public void NextVideo()
    {
        config.curClip++;

        if (config.curClip == 1)
        {
            previousButton.interactable = true;
            config.curClip = 3;
        }
        else if(config.curClip == 4)
        {
            uiBehav.ChangeScene(10);
            return;
        }
        else if (config.curClip == 5)
        {
            uiBehav.ChangeScene(8);
        }
        else if (config.curClip == videoClips.Length - 1)
        {
            nextButton.interactable = false;
        } 
        else
        {
            nextButton.interactable = true;
            previousButton.interactable = true;
        }

        CheckOnVideoButton();

        videoBehav.ChangeVideo(videoPlayer, videoClips[config.curClip], false);
    }

    public void PreviousVideo()
    {
        config.curClip--;

        if (config.curClip == -1)
        {
            uiBehav.ChangeScene(9);
            return;
        }
        else if (config.curClip == 2)
        {
            config.curClip = 0;
        }
        else if (config.curClip == 3)
        {
            uiBehav.ChangeScene(8);
        }
        else if (config.curClip == 4)
        {
            uiBehav.ChangeScene(10);
            return;
        }
        else
        {
            nextButton.interactable = true;
            previousButton.interactable = true;
        }

        CheckOnVideoButton();

        videoBehav.ChangeVideo(videoPlayer, videoClips[config.curClip], false);
    }

    void CheckOnVideoButton()
    {
        if(config.curClip == 0)
        {
            onVideoButtons[0].SetActive(true);
            onVideoButtons[1].SetActive(false);
        }
        else if (config.curClip == 3)
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
