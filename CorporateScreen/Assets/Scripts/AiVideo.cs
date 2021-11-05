using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AiVideo : MonoBehaviour
{
    //For external class
    VideoBehav videoBehav;

    [SerializeField] GameObject AiVideoCanvas;
     VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] RenderTexture renderTexture;
     Button closeButton;
    int curVideo;

    // Start is called before the first frame update
    void Start()
    {
        videoBehav = GetComponent<VideoBehav>();

        //Get VideoPlayer & Button form AiVideoCanvas
        videoPlayer = AiVideoCanvas.transform.GetChild(2).GetComponent<VideoPlayer>();
        closeButton = AiVideoCanvas.transform.GetChild(3).GetComponent<Button>();

        closeButton.onClick.AddListener(OnClickClose);
    }
    
    public void PlayVideo(int index)
    {
        curVideo = index;

        AiVideoCanvas.SetActive(true);

        videoBehav.ChangeVideo(videoPlayer, videoClips[curVideo], true);
    }

    void OnClickClose()
    {
        AiVideoCanvas.SetActive(false);

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
}
