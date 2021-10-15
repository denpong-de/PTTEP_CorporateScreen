using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : MonoBehaviour
{
    Sequence loopSequence;
    bool isMain;
    bool isOur;

    [Header("Main Canvas")]
    [SerializeField] Image mainBg;
    [SerializeField] Image mainTransition;
    [SerializeField] Image[] mainMenuBg;
    [SerializeField] Image[] mainMenuText;
    [SerializeField] RectTransform[] mainOverlays;

    [Header("About Canvas")]
    [SerializeField] Image aboutBg;
    [SerializeField] Image aboutBgText;
    [SerializeField] Image aboutTransition;
    [SerializeField] Image[] aboutMenuBg;
    [SerializeField] Image[] aboutMenuText;
    [SerializeField] RectTransform[] aboutOverlays;
    Image aboutBgWhite;

    [Header("OurBusiness Canvas")]
    [SerializeField] Image ourBg;
    [SerializeField] Image[] ourTexts;
    [SerializeField] Image ourTransition;
    [SerializeField] Image[] ourOverlays;

    [Header("OurSustain Canvas")]
    [SerializeField] Image susBgL;
    [SerializeField] Image susBgR;
    [SerializeField] Image[] susTexts;
    [SerializeField] Image susTransition;
    [SerializeField] Image[] susOverlays;

    void Awake()
    {
        aboutBgWhite = aboutBg.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    public void MainCanvasTween(float duration)
    {
        isMain = true;

        //Right background Animation when finished do Font overlay loop tween.
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(mainBg.transform.DOScale(new Vector2(0.5f, 0.5f), 0f))
            .Join(mainBg.transform.DOScale(new Vector2(1f, 1f), duration).SetEase(Ease.InOutCubic))
            .Join(mainBg.DOFade(0f, 0f))
            .Join(mainBg.DOFade(1f, duration).SetEase(Ease.InOutCubic))
            .OnComplete(FontOverlayTween);

        //White BG prevent form see through canvas
        TransitionTween(mainTransition,duration);

        //Left menu background Animation
        MenuTween(mainMenuBg, duration);
        //Left menu text Animation
        MenuTween(mainMenuText, duration);
    }

    public void AboutCanvasTween(float duration)
    {
        isMain = false;

        Vector2 stopBgPos = aboutBg.rectTransform.anchoredPosition;
        Vector2 stopBgTextPos = aboutBgText.rectTransform.anchoredPosition;

        //Right background Animation when finished do Font overlay loop tween.
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(aboutBg.transform.DOScale(new Vector3(0.4768439f, 0.3714008f, 0.3714008f), 0f))
            .Join(aboutBg.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(Ease.InOutCubic))
            .Join(aboutBg.rectTransform.DOAnchorPos(new Vector2(519f, 678f), 0f))
            .Join(aboutBg.rectTransform.DOAnchorPos(stopBgPos, duration).SetEase(Ease.OutExpo))
            .Join(aboutBgWhite.DOFade(.27f, 0f))
            .Join(aboutBgWhite.DOFade(0f, duration).SetEase(Ease.InOutCubic))
            .OnComplete(FontOverlayTween);

        //Right background text Animation
        Sequence bgTextSequence = DOTween.Sequence();
        bgTextSequence.Append(aboutBgText.DOColor(new Color(0,0,0,1),0))
            .Join(aboutBgText.DOColor(new Color(1,1,1,.42f),duration).SetEase(Ease.InOutCubic))
            .Join(aboutBgText.transform.DOScale(new Vector3(0.3755f, 0.3755f, 0.3755f),0f))
            .Join(aboutBgText.transform.DOScale(new Vector3(1,1,1),duration).SetEase(Ease.InOutCubic))
            .Join(aboutBgText.rectTransform.DOAnchorPos(new Vector2(499,493),0))
            .Join(aboutBgText.rectTransform.DOAnchorPos(stopBgTextPos, duration).SetEase(Ease.OutExpo));

        //White BG prevent form see through canvas
        TransitionTween(aboutTransition, duration);

        //Left menu background Animation
        MenuTween(aboutMenuBg, duration);
        //Left menu text Animation
        MenuTween(aboutMenuText, duration);
    }

    public void OurBusinessTween(float duration)
    {
        isOur = true;

        Vector2 stopBgPos = ourBg.rectTransform.anchoredPosition;

        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(ourBg.rectTransform.DOAnchorPos(new Vector2(0, -1833f), 0))
            .Join(ourBg.rectTransform.DOAnchorPos(stopBgPos, duration).SetEase(Ease.InOutCubic))
            .Join(ourBg.DOFade(0f, 0f))
            .Join(ourBg.DOFade(1f, duration).SetEase(Ease.InOutCubic))
            .OnComplete(FontOverlayTweenTwo);

        //White BG prevent form see through canvas
        TransitionTween(ourTransition, duration);

        TwoMenuTextTween(ourTexts,2.5f);
    }

    public void SusTween(float duration)
    {
        isOur = false;

        Vector2 stopBgPosL = susBgL.rectTransform.anchoredPosition;
        Vector2 stopBgPosR = susBgR.rectTransform.anchoredPosition;

        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(susBgL.rectTransform.DOAnchorPos(new Vector2(stopBgPosL.x, -1833f), 0))
            .Join(susBgL.rectTransform.DOAnchorPos(stopBgPosL, duration).SetEase(Ease.InOutCubic))
            .Join(susBgR.rectTransform.DOAnchorPos(new Vector2(stopBgPosR.x, -1833f), 0))
            .Join(susBgR.rectTransform.DOAnchorPos(stopBgPosR, duration).SetEase(Ease.InOutCubic))
            .Join(susBgL.DOFade(0f, 0f))
            .Join(susBgL.DOFade(1f, duration).SetEase(Ease.InOutCubic))
            .Join(susBgR.DOFade(0f, 0f))
            .Join(susBgR.DOFade(1f, duration).SetEase(Ease.InOutCubic))
            .OnComplete(FontOverlayTweenTwo);

        //White BG prevent form see through canvas
        TransitionTween(susTransition, duration);

        TwoMenuTextTween(susTexts, 2.5f);
    }

    void TransitionTween(Image image, float duration)
    {
        image.DOFade(0,0);
        image.DOFade(1f, duration);
    }

    void TwoMenuTextTween(Image[] images, float duration)
    {
        foreach (var image in images)
        {
            Vector2 startPos = image.rectTransform.anchoredPosition;

            image.rectTransform.DOAnchorPos(new Vector2(startPos.x, -1200f), 0f);
            image.rectTransform.DOAnchorPos(startPos, duration).SetEase(Ease.InOutCubic);
            image.DOFade(0f, 0f);
            image.DOFade(1f, duration).SetEase(Ease.InOutCubic);
        }
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
        RectTransform[] overlays;
        loopSequence = DOTween.Sequence();

        switch (isMain)
        {
            case true:
                overlays = mainOverlays;
                break;
            case false:
                overlays = aboutOverlays;
                break;
        }

        foreach (var overlay in overlays)
        {
            float startPos = overlay.anchoredPosition.x;

            loopSequence.Insert(0f, overlay.DOAnchorPos(new Vector2(startPos + 900f, 0f), 3f));
        }

        loopSequence.SetLoops(-1, LoopType.Restart);
    }

    void FontOverlayTweenTwo()
    {
        Image[] images;
        loopSequence = DOTween.Sequence();

        switch (isOur)
        {
            case true:
                images = ourOverlays;
                break;
            case false:
                images = susOverlays;
                break;
        }
        

        foreach (var image in images)
        {
            Vector2 startPos = image.rectTransform.anchoredPosition;

            loopSequence.Insert(0f, image.rectTransform.DOAnchorPos(new Vector2(startPos.x + 2000f, startPos.y), 3f));
        }

        loopSequence.SetLoops(-1, LoopType.Restart);
    }

    //Reset & kill loop when not using
    public void KillLoopSequence()
    {
        loopSequence.Goto(0f);
        loopSequence.Kill();
    }
}
