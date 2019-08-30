using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;

public class SavePanelManager : MonoBehaviour
{
    public GameObject SavePanel;
    public InputField NameInputField;
    public InputField IdInputField;
    public InputField CommentsInputField;
    public Image PrevieImage;
    public GameObject BottomPanel;
    public Text PreviewZoomIndication;
    // Start is called before the first frame update

    public void Awake()
    {
        EventManager.AddHandler(eEventEnum.ZoomValueChanged, new Action<double>((p_val) => {
            PreviewZoomIndication.text = "x" + p_val.ToString("F2");
        }));
    }

    public void OpenSavePanel()
    {
        PrevieImage.sprite = null;
        m_texture = null;
        m_counter += 1;
        StartCoroutine(TakeScreenshot());
    }
    
    public void SendDataAndCloseSavePanel()
    {
        BottomPanel.SetActive(true);
        SavePanel.SetActive(false);
    }
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
        showPanel();
    }


    private async Task<bool> getScreenshot()
    {
        await Task.Run(() =>
        {
            PrevieImage.sprite = m_sprite;
        });
        return true;
    }

    private void showPanel()
    {
        SavePanel.SetActive(true);
    }

    private Sprite m_sprite;
    private Texture2D m_texture;
    private int m_counter;
}
