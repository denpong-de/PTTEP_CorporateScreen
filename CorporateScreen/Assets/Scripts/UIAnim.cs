using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] RectTransform[] overlays;

    private GameObject mainBg;
    private Image mainBgImage;

    void Start()
    {
        mainBg = mainCanvas.transform.GetChild(0).gameObject;
        mainBgImage = mainBg.GetComponent<Image>();

        //For Dev.
        mainCanvasTween();
    }

    void mainCanvasTween()
    {
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(mainBg.transform.DOScale(new Vector2(0.5f, 0.5f), 0f))
            .Join(mainBg.transform.DOScale(new Vector2(1f, 1f), 2.5f).SetEase(Ease.OutExpo))
            .Join(mainBgImage.DOFade(0f, 0f))
            .Join(mainBgImage.DOFade(1f, 2.5f).SetEase(Ease.OutExpo))
            .OnComplete(FontOverlayTween);
    }

    void FontOverlayTween()
    {
        foreach (var overlay in overlays)
        {
            float startPos = overlay.anchoredPosition.x;

            overlay.DOAnchorPos(new Vector2(startPos + 900f, 0f), 3f).SetLoops(-1, LoopType.Restart);
        }
    }
}
