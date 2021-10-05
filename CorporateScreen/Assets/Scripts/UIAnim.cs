using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : MonoBehaviour
{
    [SerializeField] Image mainBg;
    [SerializeField] Image[] mainMenuBg;
    [SerializeField] Image[] mainMenuText;
    [SerializeField] RectTransform[] overlays;
    Sequence loopSequence;

    public void mainCanvasTween(float duration)
    {
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(mainBg.transform.DOScale(new Vector2(0.5f, 0.5f), 0f))
            .Join(mainBg.transform.DOScale(new Vector2(1f, 1f), duration).SetEase(Ease.InOutCubic))
            .Join(mainBg.DOFade(0f, 0f))
            .Join(mainBg.DOFade(1f, duration).SetEase(Ease.InOutCubic))
            .OnComplete(FontOverlayTween);

        foreach(var bg in mainMenuBg)
        {
            bg.rectTransform.DOAnchorPos(new Vector2(940, 0), 0f);
            bg.rectTransform.DOAnchorPos(new Vector2(0, 0), duration).SetEase(Ease.InOutCubic);
            bg.DOFade(0f,0f);
            bg.DOFade(1f, duration).SetEase(Ease.InOutCubic);
        }

        foreach(var text in mainMenuText)
        {
            Vector2 startPos = text.rectTransform.anchoredPosition;

            text.rectTransform.DOAnchorPos(new Vector2(940, startPos.y), 0f);
            text.rectTransform.DOAnchorPos(startPos, duration).SetEase(Ease.InOutCubic);
            text.DOFade(0f, 0f);
            text.DOFade(1f, duration).SetEase(Ease.InOutCubic);
        }
    }

    void FontOverlayTween()
    {
        loopSequence = DOTween.Sequence();

        foreach (var overlay in overlays)
        {
            float startPos = overlay.anchoredPosition.x;

            loopSequence.Insert(0f, overlay.DOAnchorPos(new Vector2(startPos + 900f, 0f), 3f))
                .SetLoops(-1, LoopType.Restart);
        }
    }

    public void KillLoopSequence()
    {
        loopSequence.Goto(0f);
        loopSequence.Kill();
    }
}
