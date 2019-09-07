using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using sharpPDF;


public class SavePanelManager : MonoBehaviour
{
    public GameObject SavePanel;
    public InputField NameInputField;
    public InputField IdInputField;
    public InputField CommentsInputField;
    public Image PrevieImage;
    public GameObject BottomPanel;
    public Text PreviewZoomIndication;


    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSeSP5FwgUDBEW7HaH-ZZ2TLGmzYCXIbPOcTNiXK3SLnj7Ygbw/formResponse";


    public void Awake()
    {
        EventManager.AddHandler(eEventEnum.ZoomValueChanged, new Action<double>((p_val) => {
            PreviewZoomIndication.text = "x" + p_val.ToString("F2");
        }));
    }

    public void OpenSavePanel()
    {
        PrevieImage.sprite = null;
        StartCoroutine(TakeScreenshot());
    }

    public void CancelAndCloseSavePanel()
    {
        BottomPanel.SetActive(true);
        SavePanel.SetActive(false);
    }
    
    public void SendDataAndCloseSavePanel()
    {
        //StartCoroutine(post());
        StartCoroutine(createPdf());
        
        new NativeShare().AddFile(Application.persistentDataPath + "/Data.pdf")
            .AddFile(Application.persistentDataPath + "/screenshot.png").Share();
        BottomPanel.SetActive(true);
        SavePanel.SetActive(false);
    }

    private IEnumerator createPdf()
    {
        pdfDocument myDoc = new pdfDocument("Palm Measure Application", "", false);
        pdfPage myFirstPage = myDoc.addPage();
        //yield return StartCoroutine(myFirstPage.newAddImage(Application.persistentDataPath + "/" + "screenshot.png", 0, 800));
        myFirstPage.addText("Name: " + NameInputField.text, 0, 700, sharpPDF.Enumerators.predefinedFont.csCourier, 36);
        myFirstPage.addText("Id: " + IdInputField.text, 0, 650, sharpPDF.Enumerators.predefinedFont.csCourier, 36);
        myFirstPage.addText("Scale: " + PreviewZoomIndication.text, 0, 600, sharpPDF.Enumerators.predefinedFont.csCourier, 36);
        myFirstPage.addText("Comments: " + CommentsInputField.text, 0, 550, sharpPDF.Enumerators.predefinedFont.csCourier, 36);
        myDoc.createPDF(Application.persistentDataPath + "/Data.pdf");
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        BottomPanel.SetActive(true);
        SavePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TakeScreenshot()
    {
        BottomPanel.SetActive(false);
        string imageName = "screenshot.png";

        // Take the screenshot
        ScreenCapture.CaptureScreenshot(imageName);

        //Wait for 4 frames
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }

        // Read the data from the file
        byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + imageName);

        // Create the texture
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);

        // Load the image
        screenshotTexture.LoadImage(data);

        // Create a sprite
        Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));

        // Set the sprite to the screenshotPreview
        PrevieImage.sprite = screenshotSprite;

        
        NameInputField.text = "";
        IdInputField.text = "";
        CommentsInputField.text = "";
        showPanel();
    }


    private void showPanel()
    {
        SavePanel.SetActive(true);
    }


    private IEnumerator post()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.778070382", NameInputField.text);
        form.AddField("entry.1734551411", IdInputField.text);
        form.AddField("entry.1614977173", "file:///" + Application.persistentDataPath + "/" + "screenshot.png");
        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }

    private Sprite m_sprite;
    private pdfTable m_table = new pdfTable();
}
