using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OurBusinessVideo : MonoBehaviour
{
    VideoBehav videoBehav;

    [SerializeField] GameObject videoPanel;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] VideoClip[] videoClipLoops;
    [SerializeField] GameObject controlButtonCanvas;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayVideo(int index)
    {
        curClip = index;

        videoPanel.SetActive(true);
        videoPanel.transform.SetAsLastSibling();

        if (curClip >= 4 && curClip <= 7)
        {
            controlButtonCanvas.SetActive(true);
        }
        if(curClip == 4)
        {
            previousButton.interactable = false;
        }
            
        Debug.Log("Play Video");
        videoBehav.ChangeVideo(videoPlayer,videoClips[curClip],false);
    }

    public void NextVideo()
    {
        curClip++;

        if (curClip == 7)
            nextButton.interactable = false;
        else
        {
            nextButton.interactable = true;
            previousButton.interactable = true;
        }
            

        videoBehav.ChangeVideo(videoPlayer, videoClips[curClip], false);
    }

    public void PreviousVideo()
    {
        curClip--;

        if (curClip == 4)
            previousButton.interactable = false;
        else
        {
            nextButton.interactable = true;
            previousButton.interactable = true;
        }
            

        videoBehav.ChangeVideo(videoPlayer, videoClips[curClip], false);
    }
}
