using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ScreenSaverVideo : MonoBehaviour
{
    //For external class
    [SerializeField] VideoBehav videoBehav;

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] RenderTexture renderTexture;
    int curVideo;

    void Start()
    {
        //Subscribe to event.
        GameEvents.current.onStopScreenSaver += ClearVideo;
        GameEvents.current.onStartScreenSaver += PlayVideo;

        //Subscribe video player to EndReached method when start program
        videoPlayer.loopPointReached += EndReached;
    }

    //Play loop clip when video end
    void EndReached(VideoPlayer videoPlayer)
    {
        //Make sure that video switch between 3 of them.
        curVideo = (curVideo + 1) % 3;

        //Play next clip 
        videoBehav.ChangeVideo(videoPlayer, videoClips[curVideo], false);
    }

    void PlayVideo()
    {
        //Select first video
        curVideo = 0;

        //Play first video
        videoBehav.ChangeVideo(videoPlayer,videoClips[curVideo],false);
    }

    void ClearVideo()
    {
        //stop video players
        if (videoPlayer != null)
            videoPlayer.Stop();

        //Clear render textures
        ClearOutRenderTexture(renderTexture);
    }

    //clear render texture
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

    private void OnDestroy()
    {
        //Subscribe to event.
        GameEvents.current.onStopScreenSaver -= ClearVideo;
        GameEvents.current.onStartScreenSaver -= PlayVideo;
    }
}
