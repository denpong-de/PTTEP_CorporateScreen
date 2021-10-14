using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrowserBehav : MonoBehaviour
{
    [SerializeField] OurBusinessVideo ourBusinessVideo;

    public GameObject browser;

    [SerializeField] GameObject browserCanvas;
    [SerializeField] Button closeButton;
    [SerializeField] GameObject[] backToText;
    [SerializeField] InputField urlInput;
    [SerializeField] string[] urls;

    int curUrl;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenBrowser(int urlIndex)
    {
        curUrl = urlIndex;

        browserCanvas.SetActive(true);

        browserCanvas.transform.SetAsLastSibling();

        browser.GetComponent<SimpleWebBrowser.WebBrowser2D>().Navigate(urls[urlIndex]);
        //webBrowser2D.Navigate(urls[urlIndex]);

        closeButton.onClick.AddListener(OnClickHomeButton);

        if (curUrl == 0)
            return;

        backToText[0].SetActive(true);
    }

    void OnClickHomeButton()
    {
        browserCanvas.transform.SetAsFirstSibling();

        if (curUrl == 0)
            return;

        backToText[0].SetActive(false);

        //ourBusinessVideo.PlayVideo(0);
    }
}
