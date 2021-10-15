using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrowserBehav : MonoBehaviour
{
    //For Using OurBusiness's method
    [SerializeField] OurBusinessVideo ourBusinessVideo;

    //For redirect to url
    [SerializeField] GameObject browser;
    SimpleWebBrowser.WebBrowser2D webBrowser2D;
    [SerializeField] GameObject hideImage;

    [SerializeField] GameObject browserCanvas;
    [SerializeField] Button homeButton;
    [SerializeField] GameObject[] backToText;
    [SerializeField] GameObject[] onVideoButtons;
    [SerializeField] string[] urls;

    int curUrl;

    void Start()
    {
        webBrowser2D = browser.GetComponent<SimpleWebBrowser.WebBrowser2D>();
    }

    public void OpenBrowser(int urlIndex)
    {
        curUrl = urlIndex;

        browserCanvas.SetActive(true);

        browserCanvas.transform.SetAsLastSibling();

        hideImage.SetActive(false);

        webBrowser2D.Navigate(urls[urlIndex]);

        homeButton.onClick.AddListener(OnClickHomeButton);

        if (curUrl == 0)
            return;

        backToText[0].SetActive(true);
    }

    void OnClickHomeButton()
    {
        browserCanvas.transform.SetAsFirstSibling();

        hideImage.SetActive(true);

        if (curUrl==0)
        {
            return;
        }

        backToText[0].SetActive(false);

        if (curUrl >= 7 && curUrl <= 13)
        {
            if(curUrl >= 7 && curUrl <= 9)
            {
                onVideoButtons[1].SetActive(false);
            }
            else if (curUrl >= 10 && curUrl <= 13)
            {
                onVideoButtons[2].SetActive(false);
            }
            
            onVideoButtons[0].SetActive(true);
            ourBusinessVideo.PlayVideo(0);
        }
    }
}
