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
        StartCoroutine(takeScreenshot());
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

    private IEnumerator takeScreenshot()
    {
        BottomPanel.SetActive(false);
        ScreenCapture.CaptureScreenshot("img1");
        yield return new WaitForEndOfFrame();
        string url = Application.persistentDataPath + "/img1";
        var bytes = File.ReadAllBytes(url);
        m_texture = new Texture2D(Screen.width, Screen.height);
        m_texture.LoadImage(bytes);
        m_sprite = Sprite.Create(m_texture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
        PrevieImage.sprite = m_sprite;
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
        //Thread.Sleep(100);
        SavePanel.SetActive(true);
    }

    private Sprite m_sprite;
    private Texture2D m_texture;
    private int m_counter;
}
