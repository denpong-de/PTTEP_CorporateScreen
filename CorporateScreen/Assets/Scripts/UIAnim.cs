using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : MonoBehaviour
{
    //Main Canvas
    [SerializeField] Image mainBg;
    [SerializeField] Image[] mainMenuBg;
    [SerializeField] Image[] mainMenuText;
    [SerializeField] RectTransform[] overlays;
    Sequence loopSequence;

    public void mainCanvasTween(float duration)
    {
        //Right background Animation when finished do Font overlay loop tween.
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(mainBg.transform.DOScale(new Vector2(0.5f, 0.5f), 0f))
            .Join(mainBg.transform.DOScale(new Vector2(1f, 1f), duration).SetEase(Ease.InOutCubic))
            .Join(mainBg.DOFade(0f, 0f))
            .Join(mainBg.DOFade(1f, duration).SetEase(Ease.InOutCubic))
            .OnComplete(FontOverlayTween);

        //Left menu background Animation
        MenuTween(mainMenuBg, duration);
        //Left menu text Animation
        MenuTween(mainMenuText, duration);
    }

    void MenuTween(Image[] images, float duration)
    {
        foreach (var image in images)
        {
            Vector2 startPos = image.rectTransform.anchoredPosition;

            image.rectTransform.DOAnchorPos(new Vector2(940, startPos.y), 0f);
            image.rectTransform.DOAnchorPos(startPos, duration).SetEase(Ease.InOutCubic);
            image.DOFade(0f, 0f);
            image.DOFade(1f, duration).SetEase(Ease.InOutCubic);
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

    //Reset & kill loop when not using
    public void KillLoopSequence()
    {
        loopSequence.Goto(0f);
        loopSequence.Kill();
    }
}
