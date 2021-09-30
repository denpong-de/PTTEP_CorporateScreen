using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBehav : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] RectTransform[] overlays;

    private GameObject mainBg;
    private Image mainBgImage;

    void Awake()
    {
        mainBg = mainCanvas.transform.GetChild(0).gameObject;
        mainBgImage = mainBg.GetComponent<Image>();

        mainCanvasTween();
    }

    void mainCanvasTween()
    {
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(mainBg.transform.DOScale(new Vector2(0.5f, 0.5f), 0f))
            .Insert(0,mainBgImage.DOFade(0f, 0f))
            .Insert(0,mainBgImage.DOFade(1f, 1f))
            .Insert(0,mainBg.transform.DOScale(new Vector2(1f, 1f), 1f))
            .OnComplete(FontOverlayTween);
    }

    void FontOverlayTween()
    {
        foreach(var overlay in overlays)
        {
            float startPos = overlay.anchoredPosition.x;

            overlay.DOAnchorPos(new Vector2(startPos + 900f, 0f),3f).SetLoops(-1,LoopType.Restart);
        }
    }
}
